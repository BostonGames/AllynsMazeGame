using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Heap<T> where T : IHeapItem<T>
{
    T[] items;
    int currentItemCount;

    // it is not performant to resize arrays, so we will need the maximum Heap size from our node grid
    public Heap(int maxHeapSize)
    {
        items = new T[maxHeapSize];
    }

    // for adding items to the heap
    public void Add(T item)
    {
        // each item needs to keep track of itself in the heap
        item.heapIndex = currentItemCount;
        // this adds it to the end of the array, but we need to compare it to it's parent 
        // to make sure it is in the correct position, swapping if needed
        items[currentItemCount] = item;
        SortUp(item);
        currentItemCount++;
    }

    public T RemoveFirst()
    {
        T firstItem = items[0];
        currentItemCount--;

        // want to take item from the end of the heap and put it in the first position, index [0]
        items[0] = items[currentItemCount];
        // set the heapIndex of the first item to be 0
        items[0].heapIndex = 0;
        SortDown(items[0]);
        return firstItem;
    }

    public void UpdateItem(T item)
    {
        SortUp(item);
    }

    // use an accessor to get current number of items in the heap
    public int Count
    {
        get
        {
            return currentItemCount;
        }
    }
    
    public bool Contains(T item)
    {
        return Equals(items[item.heapIndex], item);
    }

    void SortDown(T item)
    {
        while (true)
        {
            // this calculates the two binary child items on the heap parents
            int childIndexLeft = item.heapIndex * 2 + 1;
            int childIndexRight = item.heapIndex * 2 + 2;
            int swapIndex = 0;

            // this sets swapIndex to the child with the highest priority
            if(childIndexLeft < currentItemCount)
            {
                swapIndex = childIndexLeft;

                // check for a child on the right
                if (childIndexRight < currentItemCount)
                {
                    // check which of two children has a higher priority and set swap index to that child
                    if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0)
                    {
                        swapIndex = childIndexRight;
                    }
                }

                // now we check if the child has a higher priority than it's parent;
                // if so we will swap them
                if (item.CompareTo(items[swapIndex]) < 0)
                {
                    Swap(item, items[swapIndex]);
                }
                else
                {
                    // if parent has a higher priority than it's children 
                    // no swap is needed
                    return;
                }
            }
            // if the parent does not have any children,
            // it is also in it's correct position and we can exit the loop
            else { return; }
        }
    }

    private void SortUp(T item)
    {
        int parentIndex = (item.heapIndex - 1) / 2;

        while (true)
        {
            T parentItem = items[parentIndex];

            // CompareTo method:
            // high priority, returns 1
            // same priority, returns 0
            // low priority, returns -1
            if (item.CompareTo(parentItem) > 0)
            {
                // if item has got a higher priority than the parent item, we want to swap it with it's parent item
                Swap(item, parentItem);
            }
            else
            {
                // otherwise we will just keep recalculating the parent index and comparing the item to it's new parent
                break;
            }
        }
    }

    private void Swap(T itemA, T itemB)
    {
        items[itemA.heapIndex] = itemB;
        items[itemB.heapIndex] = itemA;

        // wamt to swap their heapIndexes as well
        // store first swapped in a temp var so value is not lost upone the first switch
        int itemAindex = itemA.heapIndex;
        itemA.heapIndex = itemB.heapIndex;
        itemB.heapIndex = itemAindex;
    }
}

public interface IHeapItem<T> : IComparable<T>
{
    int heapIndex
    {
        get;
        set;
    }
}
