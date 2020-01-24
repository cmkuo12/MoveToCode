﻿using UnityEngine;

namespace MoveToCode {
    public class IfCodeBlockObjectMesh : CodeBlockObjectMesh {
        Transform top, side, bottom;

        private void Awake() {
            top = transform.GetChild(0);
            side = transform.GetChild(1);
            bottom = transform.GetChild(2);
        }

        public override void AlertInstructionChanged() {
            ResizeMeshes();
        }

        public override Transform GetExitInstructionParentTransform() {
            return bottom;
        }

        private int FindChainSize() {
            return transform.parent.GetComponent<CodeBlock>().FindChainSize();
        }

        private void ResizeMeshes() {
            int chainSize = FindChainSize();
            Debug.Log("Chainsize: " + chainSize);

            Vector3 scaler = side.localScale;
            scaler.y = chainSize + 3;
            side.localScale = scaler;

            scaler = side.localPosition;
            scaler.y = -(side.localScale.y - 1) / 2;
            side.localPosition = scaler;

            // need to move down bottom also
            scaler = bottom.localPosition;
            scaler.y = -chainSize - 2;
            bottom.localPosition = scaler;
        }
    }
}
