using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EncDataModel.WPC_837I
{
    public class CLM
{
    [Key]
    public Guid id { get; set; }
    public int ordinal { get; set; }
    public Guid parentid { get; set; }
    public string CLM01 { get; set; }
    public string CLM02 { get; set; }
    public string CLM07 { get; set; }
    public string CLM08 { get; set; }
    public string CLM09 { get; set; }
    public string CLM20 { get; set; }
}
public class EDITransaction
{
    [Key]
    public Guid id { get; set; }
    public DateTime inserted_On { get; set; }
    public int ordinal { get; set; }
    public string externalCorrelationToken { get; set; }
    public string isa { get; set; }
    public string gs { get; set; }
}
public class Loops
{
    [Key]
    public Guid id { get; set; }
    public Guid parentid { get; set; }
    public Guid transactionid { get; set; }
    public string loopName { get; set; }
    public int ordinal { get; set; }
}
public class XMLDocument
{
    [Key]
    public int id { get; set; }
    public Guid transactionid { get; set; }
    [Column(TypeName="xml")]
    public string transactionDocument { get; set; }
}

}
