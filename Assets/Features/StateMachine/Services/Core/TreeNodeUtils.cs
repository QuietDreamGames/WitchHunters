using System.Linq;
using System.Reflection;
using Features.StateMachine.Components.Core;
using Unity.Entities;

namespace Features.StateMachine.Services.Core
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
                    _addComponentDataMethod = typeof(EntityManager)
                        .GetMethods()
                        .Where(x => x.Name == "AddComponentData")
                        .FirstOrDefault(x => x.ReturnType == typeof(bool));
                }

                return _addComponentDataMethod;
            }
        }

        #endregion

        #region Public methods

        public static Entity ConvertToEntity(in Entity rootEntity, ref EntityManager entityManager, TreeNode treeEntry)
        {
            var entity = ConvertLayer(in rootEntity,
                in rootEntity,
                ref entityManager,
                treeEntry,
                0,
                0,
                true);

            return entity;
        }

        #endregion

        #region Private methods

        private static Entity ConvertLayer(in Entity rootEntity,
            in Entity agentEntity,
            ref EntityManager entityManager,
            TreeNode treeNode,
            int childIndex,
            int depthIndex,
            bool isExec)
        {
            var node = treeNode.Node;

            var parent = entityManager.CreateEntity(typeof(NodeComponent), node.GetType());
            entityManager.AddComponentData(parent,
                new NodeComponent(rootEntity,
                    agentEntity,
                    childIndex,
                    depthIndex,
                    isExec));
            AddComponentDataGeneric(in parent, ref entityManager, node);

            if (treeNode.Children == default)
            {
                
            }
            else
            {
                // TODO: Entity naming and parent hierarchy
                
                //var linkedEntities = entityManager.AddBuffer<LinkedEntityGroup>(parent);
                //linkedEntities.Add(new LinkedEntityGroup { Value = parent });
                
                for (int i = 0; i < treeNode.Children.Length; i++)
                {
                    var newTreeNode = treeNode.Children[i];
                    var childEntity = ConvertLayer(in rootEntity,
                        in parent,
                        ref entityManager,
                        newTreeNode,
                        i,
                        depthIndex + 1,
                        false);

                    //linkedEntities.Add(new LinkedEntityGroup { Value = childEntity });
                }
            }

            return parent;
        }

        private static void AddComponentDataGeneric(in Entity entity, ref EntityManager entityManager, object data)
        {
            // TODO: _._ .. ._.. ._.. __ . .__. ._.. . ._ ... .
            AddComponentDataMethod
                .MakeGenericMethod(data.GetType())
                .Invoke(entityManager, new[] { entity, data });
        }

        #endregion
    }
}