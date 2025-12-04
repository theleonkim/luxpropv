using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Luxprop.Data.Models
{
    public partial class ChatMessage
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ChatMessageId { get; set; }

        public int ChatThreadId { get; set; }

        public string Sender { get; set; } = null!;

        public string Text { get; set; } = null!;

        public DateTime SentUtc { get; set; }

        public int? UsuarioId { get; set; }

        [ForeignKey(nameof(UsuarioId))]
        public virtual Usuario? Usuario { get; set; }

        [ForeignKey(nameof(ChatThreadId))]
        public virtual ChatThread ChatThread { get; set; } = null!;
    }
}
