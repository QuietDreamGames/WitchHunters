using System.Collections.Generic;
using Features.UI.Services;
using UnityEngine;

namespace Features.UI.Services
{
    public class SelectableGrid : MonoBehaviour
    {
        private List<List<ISelectable>> _selectables;
        private Vector2 _currentSelectable;

        public void AddRow()
        {
            _selectables ??= new List<List<ISelectable>>();
            
            _selectables.Add(new List<ISelectable>());
            
        }

        public void AddColumn()
        {
            _selectables ??= new List<List<ISelectable>>();

            for (int rowId = 0; rowId < _selectables.Count; rowId++)
            {
                _selectables[rowId] ??= new List<ISelectable>();
                _selectables[rowId].Add(null);
            }
        }

        public void RemoveRow(int id = -1)
        {
            _selectables ??= new List<List<ISelectable>>();
            
            if (id >= _selectables.Count || _selectables.Count == 0)
                return;

            if (id == -1)
            {
                _selectables.RemoveAt(_selectables.Count - 1);
            }
            else
            {
                _selectables.RemoveAt(id);
            }
        }

        public void RemoveColumn(int id = -1)
        {
            _selectables ??= new List<List<ISelectable>>();
            
            if (_selectables.Count == 0)
                return;
            
            if (_selectables[0].Count == 0 || id >= _selectables[0].Count)
                return;

            for (int rowId = 0; rowId < _selectables.Count; rowId++)
            {
                if (id == -1)
                {
                    _selectables[rowId].RemoveAt(_selectables[rowId].Count - 1);
                }
                else
                {
                    _selectables[rowId].RemoveAt(id);
                }
            }
        }

        public void AddHorizontalElement(ISelectable selectable,  int row = -1, bool toEnd = false)
        {
            _selectables ??= new List<List<ISelectable>>();
            _selectables[0] ??= new List<ISelectable>();
            // Replace the first null value in list going horizontally
            if (!toEnd && row == -1)
            {
                var isAdded = false;
                for (int rowId = 0; rowId < _selectables.Count; rowId++)
                {
                    
                    for (int columnId = 0; columnId < _selectables[rowId].Count; columnId++)
                    {
                        if (_selectables[rowId][columnId] == null)
                        {
                            _selectables[rowId][columnId] = selectable;
                            isAdded = true;
                        }
                    }

                    if (isAdded) break;
                }

                if (!isAdded)
                {
                    _selectables[^1].Add(selectable);
                }
            }
            // Add new element at the last row
            else if (toEnd && row == -1)
            {
                _selectables[^1].Add(selectable);
            }
            // Replace the first null value in a certain row (horizontally)
            else if (!toEnd)
            {
                
            }
            // Add new element at the certain row
            else
            {
                
            }
            
            BalanceRows();
        }
        
        public void AddVerticalElement(ISelectable selectable, int column = -1, bool toEnd = false)
        {
            _selectables ??= new List<List<ISelectable>>();
            _selectables[0] ??= new List<ISelectable>();
            
            for (int rowId = 0; rowId < _selectables.Count; rowId++)
            {
                var isAdded = false;
                for (int columnId = 0; columnId < _selectables[rowId].Count; columnId++)
                {
                    if (_selectables[rowId][columnId] == null)
                    {
                        _selectables[rowId][columnId] = selectable;
                        isAdded = true;
                    }
                }
                if (isAdded) break;
            }
            
            BalanceRows();
        }

        public void DeleteElement(ISelectable selectable = null)
        {
            
        }
        
        public void DeleteElement(int row = -1, int column = -1)
        {
            
        }

        public void SelectNextX()
        {
            
        }
        
        public void SelectNextY()
        {
            
        }
        
        public void SelectPreviousX()
        {
            
        }
        
        public void SelectPreviousY()
        {
            
        }

        public void Enter()
        {
            
        }

        private void BalanceRows()
        {
            
        }
    }
}