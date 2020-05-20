using System;
using System.Runtime.Serialization;

namespace ECommerce.Domains.Inventory
{
    public class InventoryItemId : IFormattable, IComparable, IComparable<InventoryItemId>, IEquatable<InventoryItemId>
    {
        private Guid id;

        public InventoryItemId() => this.id = Guid.NewGuid();

        public int CompareTo(object obj) => this.id.CompareTo(((InventoryItemId)obj).id);

        public int CompareTo(InventoryItemId other) => this.id.CompareTo(other.id);

        public bool Equals(InventoryItemId other) => this.id.Equals(other.id);

        public string ToString(string format,
                               IFormatProvider formatProvider) => this.id.ToString(format, formatProvider);

        public static bool operator ==(InventoryItemId item1,
                                       InventoryItemId item2) => item1.Equals(item2);

        public static bool operator !=(InventoryItemId item1,
                                       InventoryItemId item2) => !item1.Equals(item2);

        public override bool Equals(object obj) => (obj is InventoryItemId) ? this.id.Equals(((InventoryItemId)obj).id) : false;

        public override int GetHashCode() => this.id.GetHashCode();

        public override string ToString() => this.id.ToString();

        public string ToString(string format) => this.id.ToString(format);
    }
}