using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Luxprop.Data.Models
{
    public partial class ChatThread
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ChatThreadId { get; set; }

        public string State { get; set; } = null!;

        public string? ClientName { get; set; }

        public string? ClientEmail { get; set; }

        public DateTime CreatedUtc { get; set; }

        public DateTime? ClosedUtc { get; set; }

        public bool NeedsAgent { get; set; }

        public int? CreatedByUserId { get; set; }

        [ForeignKey(nameof(CreatedByUserId))]
        public virtual Usuario? UsuarioCreador { get; set; }

        public virtual ICollection<ChatMessage> ChatMessages { get; set; } = new List<ChatMessage>();
    }
}
