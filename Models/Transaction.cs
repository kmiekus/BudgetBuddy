using System.ComponentModel.DataAnnotations;

namespace BudgetBuddy.Models;

/// <summary>
/// Represents a financial transaction record with immutable properties.
/// </summary>
/// <remarks>
/// This record maintains basic transaction information including a unique identifier,
/// name, amount, type, and timestamp of the transaction.
/// </remarks>
/// <property name="Id">A unique identifier for the transaction. Automatically generated on creation.</property>
/// <property name="Name">The descriptive name of the transaction.</property>
/// <property name="Amount">The monetary value of the transaction.</property>
/// <property name="Type">The category or type of the transaction.</property>
/// <property name="Date">The timestamp when the transaction was created. Defaults to current date and time.</property>
public record Transaction
{
    public Guid Id { get; init; } = Guid.NewGuid();
  
    [Required(ErrorMessage = "Nazwa jest wymagana")]
    public required string Name { get; set; }

    [Required(ErrorMessage = "Kwota jest wymagana")]
    [Range(0.01, 1000000, ErrorMessage = "Kwota musi być większa niż 0")]
    public required decimal Amount { get; set; }
    public required TransactionType Type { get; set; }

    public DateTime Date { get; init; } = DateTime.Now;
}