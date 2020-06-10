﻿using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class ArrayElementsObjectMesh : CodeBlockObjectMesh {
        //Transform top;
        //List<Transform> elements;
        Transform[] elements;
        int numElements;
        //Transform top, argLeft, argRight;
        Vector3 origScaleArg;
        //Vector3 origPositionArgLeft, origPositionArgRight;
        //List<Vector3> origPositionElements;
        Vector3[] origPositionElements;

        public override void SetUpObject() {
            numElements = (this.transform.parent.GetComponent<ArrayCodeBlock>().GetMyInternalIArgument() as ArrayDataStructure).GetSize();
            SetUpElements();
            AddSnapColliderGroupScript();
            RepositionElements();
        }

        private void SetUpElements() {
            elements = new Transform[numElements];
            InstantiateElements();
            //Fill elements array with elements' transforms
            for (int i = 0; i < elements.Length; i++) {
                elements[i] = transform.GetChild(i);
            }
            SetOriginalScale();
            SetUpOriginalPositions();
            SetElementArgPositions();
        }

        private void SetUpOriginalPositions() {
            origPositionElements = new Vector3[numElements];
            for (int i = 0; i < numElements; i++) {
                origPositionElements[i] = elements[i].localPosition;
            }
        }

        private void SetOriginalScale() {
            origScaleArg = elements[elements.Length - 1].localScale;
        }

        private void SetElementArgPositions() {
            for(int i = 0; i < numElements; i++) {
                elements[i].GetChild(0).GetComponent<SnapCollider>().SetMyArgumentPosition(i);
            }
        }

        private void InstantiateElements() {
            for (int i = 0; i < numElements; i++) {
                GameObject ElementGameObject = Instantiate(
                    Resources.Load<GameObject>(ResourcePathConstants.ElementCodeBlockPrefab), GetMyCodeBlock().transform) as GameObject;
                ElementGameObject.SetActive(false);
                ElementGameObject.transform.SnapToParent(this.transform);
                ElementGameObject.SetActive(true);
            }
        }

        private void RepositionElements() {
            for (int i = 1; i < numElements; i++) {
                elements[i].localPosition = new Vector3(elements[i - 1].localPosition.x + 0.5f, elements[i - 1].localPosition.y, elements[i - 1].localPosition.z);
            }
        }

        private void AddSnapColliderGroupScript() {
            transform.gameObject.AddComponent<SnapColliderGroup>();
        }

        public override void SetUpMeshOutlineList() {
            /*meshOutlineList = new List<MeshOutline>() {
                top.gameObject.AddComponent<MeshOutline>(),
                argLeft.gameObject.AddComponent<MeshOutline>(),
                argRight.gameObject.AddComponent<MeshOutline>()
                };*/
            meshOutlineList = new List<MeshOutline>() { };
            //meshOutlineList.Add(top.gameObject.AddComponent<MeshOutline>());
            for (int i = 0; i < elements.Length; i++) {
                meshOutlineList.Add(elements[i].gameObject.AddComponent<MeshOutline>());
            }
        }

        public override float GetBlockHorizontalSize() {
            float size = 0;
            for (int i = 0; i < elements.Length; i++) {
                size += elements[i].localScale.x;
            }
            return size;
                //return argRight.localScale.x + argLeft.localScale.x + top.localScale.x;
        }

        public override float GetBlockVerticalSize() {
            return transform.localScale.y;
        }

        // left bounds minus right 
        public override Vector3 GetCenterPosition() {
            Vector3 leftmostB = elements[0].localPosition;
            leftmostB.x -= (elements[0].localScale.x / 2.0f);
            Vector3 rightmostB = elements[elements.Length-1].localPosition;
            rightmostB.x += (elements[elements.Length-1].localScale.x / 2.0f);

            return (rightmostB + leftmostB) / 2.0f;
        }

        protected override void ResizeObjectMesh() {
            //ResizeArgLeft();
            //ResizeArgRight();
            ResizeElements();
        }

        // private methods

        //TODO: Fix this
        private void ResizeElements() {
            /*// need to resize arg right based upon horizontal size of arg
            Vector3 rescale = origScaleArg;
            //List<Vector3> reposition = origPositionElements;
            Vector3[] reposition = origPositionElements;//.ToArray();
            for(int i = 0; i < origPositionElements.Length; i++) {
                float? horizontalSize = GetMyCodeBlock().GetArgAsCodeBlockAt(i)?.GetCodeBlockObjectMesh().GetBlockHorizontalSize();
                if (horizontalSize != null) {
                    rescale.x = (float)horizontalSize;
                    reposition[i].x = reposition[i].x - (rescale.x - 0.5f) / 2.0f;
                }
                elements[i].localPosition = reposition[i];
                elements[i].localScale = rescale;
            }
            //Vector3 reposition = origPositionArgLeft;*/
        }

        /*private void ResizeArgLeft() {
            // need to resize arg right based upon horizontal size of arg
            Vector3 rescale = origScaleArg;
            Vector3 reposition = origPositionElements;
            //Vector3 reposition = origPositionArgLeft;
            float? horizontalSize = GetMyCodeBlock().GetArgAsCodeBlockAt(0)?.GetCodeBlockObjectMesh().GetBlockHorizontalSize();
            if (horizontalSize != null) {
                rescale.x = (float)horizontalSize;
                reposition.x = reposition.x - (rescale.x - 0.5f) / 2.0f;
            }
            argLeft.localPosition = reposition;
            argLeft.localScale = rescale;
        }

        private void ResizeArgRight() {
            // need to resize arg right based upon horizontal size of arg
            Vector3 rescale = origScaleArg;
            Vector3 reposition = origPositionArgRight;
            float? horizontalSize = GetMyCodeBlock().GetArgAsCodeBlockAt(1)?.GetCodeBlockObjectMesh().GetBlockHorizontalSize();
            if (horizontalSize != null) {
                rescale.x = (float)horizontalSize;
                // 
                reposition.x = reposition.x + (rescale.x - 0.5f) / 2.0f;
            }
            argRight.localPosition = reposition;
            argRight.localScale = rescale;
        }*/
    }
}
