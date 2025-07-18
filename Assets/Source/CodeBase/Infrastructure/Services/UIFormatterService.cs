using Source.CodeBase.Infrastructure.Services.Interfaces;

namespace Source.CodeBase.Infrastructure.Services
{
  public class UIFormatterService : IUIFormatterService
  {
    public string FormatCooldownText(int seconds, string format) =>
      string.Format(format ?? "{0}", seconds);
  }
}