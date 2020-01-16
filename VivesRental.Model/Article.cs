using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VivesRental.Model
{
    [Table(nameof(Article))]
    public class Article
    {
        public Article()
        {
            OrderLines = new List<OrderLine>();
        }

        [Key] public Guid Id { get; set; }

        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public ArticleStatus Status { get; set; }

        public IList<OrderLine> OrderLines { get; set; }


        // To make except() work on List<Article>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Article))
                throw new ArgumentException("obj is not an Article");
            var usr = obj as Article;
            if (usr == null)
                return false;
            return Id.Equals(usr.Id);
        }
    }

    public enum ArticleStatus
    {
        Normal = 0,
        Broken = 1,
        InRepair = 2,
        Lost = 3,
        Destroyed = 4
    }
}