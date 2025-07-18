namespace Source.CodeBase.Infrastructure.Services.Interfaces
{
  public interface IUIFormatterService
  {
    string FormatCooldownText(int seconds, string format);
  }
}