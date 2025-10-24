using FactElectFixed.Api.Database;
using FactElectFixed.Api.Features.Configuracion.Entities;
using Infoware.SRI.Firmar;
using Microsoft.EntityFrameworkCore;
using NuGet.ProjectModel;

namespace FactElectFixed.Api.Features.Configuracion.Services;

public class ConfiguracionService(
    ApplicationDbContext dbContext,
    ICertificadoService certificadoService,
    IWebHostEnvironment webHostEnvironment)
    : IConfiguracionService
{
    private static readonly Lock FileLock = new();

    public async Task<ConfiguracionEntity> SaveConfiguracion(string ruc, string password, IFormFile archivoP12,
        CancellationToken ct = default)
    {
        if (await dbContext.Configuraciones.AnyAsync(c => c.RucEmpresa == ruc, ct))
        {
            throw new ApplicationException("Ya existe una configuracion asociada al RUC ingresado.");
        }

        byte[] fileBytes = await ConvertToBytes(archivoP12);
        VerificarPasswordP12(password, fileBytes);

        await GuardarArchivoP12(ruc, archivoP12, ct);

        var configuracion = new ConfiguracionEntity
        {
            Password = password,
            RucEmpresa = ruc
        };

        await GuardarConfiguracionDb(configuracion, ct);

        return configuracion;
    }

    public async Task<ConfiguracionEntity> UpdateConfiguracion(string ruc, string password, IFormFile archivoP12,
        CancellationToken ct = default)
    {
        if (!await dbContext.Configuraciones.AnyAsync(c => c.RucEmpresa == ruc, ct))
        {
            throw new ApplicationException("No existe una configuracion registrada con el RUC ingresado.");
        }

        byte[] fileBytes = await ConvertToBytes(archivoP12);
        VerificarPasswordP12(password, fileBytes);

        await GuardarArchivoP12(ruc, archivoP12, ct);

        return await ActualizarConfiguracionPasswordDb(ruc, password, ct);
    }

    private async Task<byte[]> ConvertToBytes(IFormFile file)
    {
        if (file.Length == 0)
        {
            return null;
        }

        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);
        return ms.ToArray();
    }

    private static Task SaveP12FileAsync(IFormFile file, string path, CancellationToken ct = default)
    {
        if (file == null || file.Length == 0)
        {
            throw new ArgumentException("Archivo inválido.");
        }

        Directory.CreateDirectory(Path.GetDirectoryName(path)!);

        string tempFile = path + ".tmp";

        lock (FileLock)
        {
            using (var fs = new FileStream(tempFile, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                file.CopyToAsync(fs, ct);
            }

            File.Copy(tempFile, path, true);
            File.Delete(tempFile);
        }

        return Task.CompletedTask;
    }

    private void VerificarPasswordP12(string password, byte[] fileBytes)
    {
        try
        {
            certificadoService.CargarDesdeBytes(fileBytes, password);
        }
        catch (Exception e)
        {
            throw new FileLoadException($"Archivo y/o contrasena incorrectos: {e.Message}");
        }
    }

    private async Task GuardarArchivoP12(string ruc, IFormFile archivoP12, CancellationToken ct = default)
    {
        try
        {
            string certPath = Path.Combine(webHostEnvironment.ContentRootPath, "Files", ruc);

            if (!Directory.Exists(certPath))
            {
                Directory.CreateDirectory(certPath);
            }

            string certSavePath = Path.Combine(certPath, "cert.p12");

            await SaveP12FileAsync(archivoP12, certSavePath, ct);
        }
        catch (Exception)
        {
            throw new FileFormatException(
                "Archivo y Contrasena correctos pero ocurrio un error al guardar el archivo P12.");
        }
    }

    private async Task GuardarConfiguracionDb(ConfiguracionEntity configuracion, CancellationToken ct = default)
    {
        await dbContext.Configuraciones.AddAsync(configuracion, ct);

        await dbContext.SaveChangesAsync(ct);
    }

    private async Task<ConfiguracionEntity> ActualizarConfiguracionPasswordDb(string ruc, string password,
        CancellationToken ct = default)
    {
        ConfiguracionEntity configuracion =
            await dbContext.Configuraciones.FirstOrDefaultAsync(c => c.RucEmpresa == ruc, ct);

        configuracion!.Password = password;
        await dbContext.SaveChangesAsync(ct);

        return configuracion;
    }
}
