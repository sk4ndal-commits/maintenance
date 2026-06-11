using QRCoder;

namespace MaintenanceSystem.Infrastructure.Services;

public interface IQrCodeService
{
    byte[] GeneratePng(string payload, int pixelsPerModule = 20);
}

public class QrCodeService : IQrCodeService
{
    public byte[] GeneratePng(string payload, int pixelsPerModule = 20)
    {
        using var generator = new QRCodeGenerator();
        var data = generator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);
        var qrCode = new PngByteQRCode(data);
        return qrCode.GetGraphic(pixelsPerModule);
    }
}
