﻿/// <summary>
/// Linq extensions
/// </summary>
namespace Hackathon.MLBox.Foundation.Common.Extensions
{
    using System;
    using System.Collections.Generic;

    public static class LinqExtensions
    {

        /// <summary>
        /// Split the list into parts
        /// </summary>
        public static List<T>[] Partition<T>(this List<T> list, int totalPartitions)
        {
            if (list == null)
                throw new ArgumentNullException("list");

            if (totalPartitions < 1)
                throw new ArgumentOutOfRangeException("totalPartitions");

            List<T>[] partitions = new List<T>[totalPartitions];

            int maxSize = (int)Math.Ceiling(list.Count / (double)totalPartitions);
            int k = 0;

            for (int i = 0; i < partitions.Length; i++)
            {
                partitions[i] = new List<T>();
                for (int j = k; j < k + maxSize; j++)
                {
                    if (j >= list.Count)
                        break;
                    partitions[i].Add(list[j]);
                }
                k += maxSize;
            }

            return partitions;
        }
    }
}