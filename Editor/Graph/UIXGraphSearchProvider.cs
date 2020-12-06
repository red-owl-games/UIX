using System;
using System.Collections.Generic;
using System.Reflection;
using RedOwl.UIX.Engine;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace RedOwl.UIX.Editor
{
    public struct SearchGroupKey
    {
        private readonly string _name;
        private readonly int _depth;

        public SearchGroupKey(string name, int depth)
        {
            _name = name;
            _depth = depth;
        }

        public override int GetHashCode() => _name.GetHashCode() + _depth.GetHashCode();
    }
    
    public class SearchGroup
    {
        public SearchTreeGroupEntry Section { get; }
        public List<SearchTreeEntry> Entries { get; }
        
        public SearchGroup(string name, int depth)
        {

            Section = new SearchTreeGroupEntry(new GUIContent(name), depth);
            Entries = new List<SearchTreeEntry>();
        }

        public void Add(UIXNodeReflection data)
        {
            Entries.Add(new SearchTreeEntry(new GUIContent(data.Name)){ userData = data, level = Section.level + 1});
        }
    }
    
    public class UIXGraphSearchProvider : ScriptableObject, ISearchWindowProvider
    {
        private UIXGraphView _view;
        private bool _useGraphTagMatching;
        private HashSet<string> _graphTags;

        public void Initialize(UIXGraphView view)
        {
            _view = view;
            // TODO: cache this?
            var attr = _view.Graph.GetType().GetCustomAttribute<GraphAttribute>();
            if (attr == null) return;
            _useGraphTagMatching = true;
            _graphTags = new HashSet<string>(attr.Tags);;
        }
        
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var tree = new List<SearchTreeEntry>
            {
                new SearchTreeGroupEntry(new GUIContent("Create Node"), 0),
            };
            foreach (var group in GetSearchGroups())
            {
                tree.Add(group.Section);
                foreach (var entry in group.Entries)
                {
                    tree.Add(entry);
                }
            }
            return tree;
        }

        public bool OnSelectEntry(SearchTreeEntry entry, SearchWindowContext context)
        {
            _view.CreateNode((UIXNodeReflection)entry.userData, context.screenMousePosition);
            return true;
        }

        private IEnumerable<SearchGroup> GetSearchGroups()
        {
            Dictionary<SearchGroupKey, SearchGroup> groups = new Dictionary<SearchGroupKey, SearchGroup>();
            foreach (var node in UIXReflector.NodeCache.All)
            {
                if (_useGraphTagMatching && !_graphTags.IsSubsetOf(node.Tags)) continue;
                SearchGroup searchGroup = null;
                int depth = 1;
                
                foreach (string subsection in node.Path)
                {
                    var key = new SearchGroupKey(subsection, depth);
                    if (!groups.TryGetValue(key, out searchGroup))
                    {
                        searchGroup = new SearchGroup(subsection, depth);
                        groups.Add(key, searchGroup);
                    }
                    depth++;
                }

                searchGroup?.Add(node);
            }

            var data = new List<SearchGroup>(groups.Values);
            data.Sort((a, b) => string.Compare(a.Section.name, b.Section.name, StringComparison.Ordinal));
            foreach (var group in data)
            {
                yield return group;
            }
        }
    }
}