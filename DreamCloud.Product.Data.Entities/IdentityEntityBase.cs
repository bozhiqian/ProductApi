using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DreamCloud.Product.Data.Entities
{
    public abstract class IdentityEntityBase<T> : IIdentity<T>
    {
        [Key] public T Id { get; set; }

        #region Auditing

        [Column(Order = 101)]
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedDateUtc { get; set; }

        [Column(Order = 103)]
        public DateTime? ModifiedDateUtc { get; set; }

        #endregion
    }
}
