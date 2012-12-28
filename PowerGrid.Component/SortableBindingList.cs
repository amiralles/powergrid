namespace PowerGrid.Component {
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    //this code belong to http://www.timvw.be/2008/08/02/presenting-the-sortablebindinglistt-take-two/
    public class SortableBindingList<T> : BindingList<T> {
        private readonly Dictionary<Type, PropertyComparer<T>> _comparers;
        private bool _isSorted;
        private ListSortDirection _listSortDirection;
        private PropertyDescriptor _propertyDescriptor;

        public SortableBindingList()
            : base(new List<T>()) {
            _comparers = new Dictionary<Type, PropertyComparer<T>>();
        }

        public SortableBindingList(IEnumerable<T> enumeration)
            : base(new List<T>(enumeration)) {
            _comparers = new Dictionary<Type, PropertyComparer<T>>();
        }

        protected override bool SupportsSortingCore {
            get { return true; }
        }

        protected override bool IsSortedCore {
            get { return _isSorted; }
        }

        protected override PropertyDescriptor SortPropertyCore {
            get { return _propertyDescriptor; }
        }

        protected override ListSortDirection SortDirectionCore {
            get { return _listSortDirection; }
        }

        protected override bool SupportsSearchingCore {
            get { return true; }
        }

        protected override void ApplySortCore(PropertyDescriptor property, ListSortDirection direction) {
            var itemsList = (List<T>)Items;
            var propertyType = property.PropertyType;
            PropertyComparer<T> comparer;
            if (!_comparers.TryGetValue(propertyType, out comparer)) {
                comparer = new PropertyComparer<T>(property, direction);
                _comparers.Add(propertyType, comparer);
            }

            comparer.SetPropertyAndDirection(property, direction);
            itemsList.Sort(comparer);

            _propertyDescriptor = property;
            _listSortDirection = direction;
            _isSorted = true;

            OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }

        protected override void RemoveSortCore() {
            _isSorted = false;
            _propertyDescriptor = base.SortPropertyCore;
            _listSortDirection = base.SortDirectionCore;

            OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }

        protected override int FindCore(PropertyDescriptor property, object key) {
            var count = Count;
            for (var i = 0; i < count; ++i) {
                var element = this[i];
                var value = property.GetValue(element);
                if (value != null && value.Equals(key)) {
                    return i;
                }
            }

            return -1;
        }
    }
}