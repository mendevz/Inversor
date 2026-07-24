
using System.ComponentModel.DataAnnotations;

namespace Inversor.Infrastructure.Options;

public class RabbitMqOptions
{
    public const string SectionName = "RabbitMQ";

    [Required(ErrorMessage = "RabbitMQ Host is required.")]
    public string Host { get; set; } = "localhost";

    [Range(1, 65535, ErrorMessage = "RabbitMQ Port must be a valid port number.")]
    public ushort Port { get; set; } = 5672;

    [Required(ErrorMessage = "RabbitMQ Username is required.")]
    public string Username { get; set; } = "guest";

    [Required(ErrorMessage = "RabbitMQ Password is required.")]
    public string Password { get; set; } = "guest";
}
