using System.Collections.Generic;
using UnityEngine;

namespace DrawIn3D
{
    [DefaultExecutionOrder(100)]
    [DisallowMultipleComponent]
    public class PaintableManager : MonoBehaviour
    {
        private static readonly LinkedList<PaintableManager> Instances = new();
        private LinkedListNode<PaintableManager> _instancesNode;

        private static int _activePaintCount;

        public static void MarkActivelyPainting() => _activePaintCount += 2;

        public static void GetOrCreateInstance()
        {
            var paintableManager = new GameObject(nameof(PaintableManager));

            paintableManager.AddComponent<PaintableManager>();
        }

        public static void SubmitAll(Command command)
        {
            var models = Model.FindOverlap();

            for (var i = models.Count - 1; i >= 0; i--)
                SubmitAll(command, models[i]);
        }

        protected virtual void OnEnable() =>
            _instancesNode = Instances.AddLast(this);

        protected virtual void OnDisable()
        {
            Instances.Remove(_instancesNode);
            _instancesNode = null;
        }

        protected virtual void LateUpdate()
        {
            UpdateAll();

            _activePaintCount = _activePaintCount switch
            {
                > 1 => 1,
                1 => 0,
                _ => _activePaintCount
            };
        }

        private static void SubmitAll(Command command, Model model)
        {
            var paintableTextures = model.FindPaintableTextures();

            for (var i = paintableTextures.Count - 1; i >= 0; i--)
                Submit(command, model, paintableTextures[i]);
        }

        private static void Submit(Command command, Model model, PaintableTexture paintableTexture)
        {
            var copy = command.SpawnCopy();

            copy.Apply();

            copy.Model = model;
            copy.Submesh = 0;

            paintableTexture.AddCommand(copy);
        }
        
        private static void UpdateAll()
        {
            foreach (var paintableTexture in PaintableTexture.Textures)
                paintableTexture.ExecuteCommands();
        }
    }
}