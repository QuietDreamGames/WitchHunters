using System.Linq;
using System.Reflection;
using Features.BehaviourTree.Components.Core;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace Features.BehaviourTree.Services.Core
{
    public static class TreeNodeUtils
    {
        #region Private fields

        private static MethodInfo _addComponentDataMethod;

        private static MethodInfo AddComponentDataMethod
        {
            get
            {
                if (_addComponentDataMethod == null)
                {
                    _addComponentDataMethod = typeof(EntityCommandBuffer)
                        .GetMethods()
                        .Where(x => x.Name == "AddComponent")
                        .FirstOrDefault(x => x.ReturnType == typeof(void));
                }

                return _addComponentDataMethod;
            }
        }

        #endregion

        #region Public methods

        public static Entity ConvertToEntity(in Entity rootEntity, ref EntityManager entityManager, TreeNode treeEntry)
        {
            var entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);
            
            var entity = ConvertLayer(in rootEntity,
                in rootEntity,
                ref entityManager,
                ref entityCommandBuffer,
                treeEntry,
                0,
                0,
                true);
            
            entityCommandBuffer.AddComponent(entity, new Parent { Value = rootEntity });

            entityCommandBuffer.Playback(entityManager);
            entityCommandBuffer.Dispose();

            return entity;
        }

        #endregion

        #region Private methods

        private static Entity ConvertLayer(in Entity rootEntity,
            in Entity agentEntity,
            ref EntityManager entityManager,
            ref EntityCommandBuffer entityCommandBuffer,
            TreeNode treeNode,
            int childIndex,
            int depthIndex,
            bool isExec)
        {
            var node = treeNode.Node;

            var parentArchetype = entityManager.CreateArchetype(typeof(NodeComponent), node.GetType());
            var parent = entityCommandBuffer.CreateEntity(parentArchetype);
            entityCommandBuffer.SetName(parent, $"[{childIndex}]:{GetNodeDescription(treeNode)}");
            
            entityCommandBuffer.AddComponent(parent,
                new NodeComponent(rootEntity,
                    agentEntity,
                    childIndex,
                    depthIndex,
                    isExec));
            AddComponentDataGeneric(in parent, ref entityCommandBuffer, node);

            if (treeNode.Children == default)
            {
                
            }
            else
            {
                // TODO: Entity naming and parent hierarchy

                for (int i = 0; i < treeNode.Children.Length; i++)
                {
                    var newTreeNode = treeNode.Children[i];
                    var childEntity = ConvertLayer(in rootEntity,
                        in parent,
                        ref entityManager,
                        ref entityCommandBuffer,
                        newTreeNode,
                        i,
                        depthIndex + 1,
                        false);

                    entityCommandBuffer.AddComponent(childEntity, new Parent { Value = parent });
                }
            }

            return parent;
        }

        private static void AddComponentDataGeneric(in Entity entity, ref EntityCommandBuffer entityCommandBuffer, object data)
        {
            // TODO: _._ .. ._.. ._.. __ . .__. ._.. . ._ ... .
            AddComponentDataMethod
                .MakeGenericMethod(data.GetType())
                .Invoke(entityCommandBuffer, new[] { entity, data });
        }

        private static string GetNodeType(object node)
        {
            return node
                .GetType()
                .ToString()
                .Split(".")[^1];
        }

        public static string GetNodeDescription(TreeNode treeNode)
        {
            return treeNode.Description == default 
                ? GetNodeType(treeNode.Node)
                : $"{treeNode.Description}";
        }

        #endregion
    }
}