﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Kean.Infrastructure.Utilities
{
    /// <summary>
    /// 树结构
    /// </summary>
    /// <typeparam name="T">树中元素的类型</typeparam>
    [DataContract]
    public sealed class Tree<T>
    {
        /// <summary>
        /// 初始化 Aiwms.Implement.Base.Utiliy.Tree 类的新实例
        /// </summary>
        public Tree() { }

        /// <summary>
        /// 初始化 Aiwms.Implement.Base.Utiliy.Tree 类的新实例
        /// </summary>
        /// <param name="data">数据元素</param>
        public Tree(T data)
        {
            Data = data;
        }

        /// <summary>
        /// 初始化 Aiwms.Implement.Base.Utiliy.Tree 类的新实例
        /// </summary>
        /// <param name="items">包含用于标识层级关系属性的数据集合</param>
        /// <param name="keyField">数据标识属性</param>
        /// <param name="parentField">层级标识属性</param>
        public Tree(IEnumerable<T> items, string keyField, string parentField)
        {
            if (items != null)
            {
                Type type = typeof(T);
                PropertyInfo keyProperty = type.GetProperty(keyField);
                PropertyInfo parentProperty = type.GetProperty(parentField);
                Dictionary<object, Tree<T>> dic = new Dictionary<object, Tree<T>>();
                foreach (T item in items)
                {
                    dic.Add(keyProperty.GetValue(item, null), new Tree<T>(item));
                }
                foreach (T item in items)
                {
                    Tree<T> node = dic[keyProperty.GetValue(item, null)];
                    object parentValue = parentProperty.GetValue(item, null);
                    if (dic.ContainsKey(parentValue))
                    {
                        dic[parentValue].AppendChild(node);
                    }
                    else
                    {
                        AppendChild(node);
                    }
                }
                foreach (Tree<T> node in GetDescendants())
                {
                    node.Level = node.Parent.Level + 1;
                }
            }
        }

        /// <summary>
        /// 数据元素
        /// </summary>
        [DataMember]
        public T Data { get; set; }

        /// <summary>
        /// 层（Root为0）
        /// </summary>
        [DataMember]
        public int Level { get; private set; }

        /// <summary>
        /// 双亲
        /// </summary>
        [JsonIgnore]
        public Tree<T> Parent { get; private set; }

        /// <summary>
        /// 孩子
        /// </summary>
        [DataMember]
        public IList<Tree<T>> Children { get; private set; }

        /// <summary>
        /// 添加孩子
        /// </summary>
        /// <param name="tree">孩子</param>
        public void AppendChild(Tree<T> tree)
        {
            tree.Level = Level + 1;
            tree.Parent = this;
            if (Children == null)
            {
                Children = new List<Tree<T>>();
            }
            Children.Add(tree);
        }

        /// <summary>
        /// 移除孩子
        /// </summary>
        /// <param name="tree">孩子</param>
        public void RemoveChild(Tree<T> tree)
        {
            if (Children != null && Children.Contains(tree))
            {
                Children.Remove(tree);
            }
        }

        /// <summary>
        /// 获取祖先
        /// </summary>
        /// <returns>祖先</returns>
        public IEnumerable<Tree<T>> GetAncestors()
        {
            if (Parent != null)
            {
                yield return Parent;
                foreach (Tree<T> ancestor in Parent.GetAncestors())
                {
                    yield return ancestor;
                }
            }
        }

        /// <summary>
        /// 获取指定数据元素的祖先
        /// </summary>
        /// <param name="data">数据元素</param>
        /// <returns>祖先</returns>
        public Tree<T> GetAncestor(T data)
        {
            if (Parent == null)
            {
                return null;
            }
            else
            {
                if (Parent.Equals(data))
                {
                    return Parent;
                }
                else
                {
                    return Parent.GetAncestor(data);
                }
            }
        }

        /// <summary>
        /// 获取后代
        /// </summary>
        /// <returns>后代</returns>
        public IEnumerable<Tree<T>> GetDescendants()
        {
            if (Children != null)
            {
                foreach (Tree<T> child in Children)
                {
                    yield return child;
                    foreach (Tree<T> descendant in child.GetDescendants())
                    {
                        yield return descendant;
                    }
                }
            }
        }

        /// <summary>
        /// 获取指定数据元素的后代
        /// </summary>
        /// <param name="data">数据元素</param>
        /// <returns>后代</returns>
        public Tree<T> GetDescendant(T data)
        {
            if (Children == null)
            {
                return null;
            }
            else
            {
                foreach (Tree<T> child in Children)
                {
                    if (child.Data.Equals(data))
                    {
                        return child;
                    }
                    else
                    {
                        Tree<T> descendant = child.GetDescendant(data);
                        if (descendant != null)
                        {
                            return descendant;
                        }
                    }
                }
                return null;
            }
        }
    }
}
