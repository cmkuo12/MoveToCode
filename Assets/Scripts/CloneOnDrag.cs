﻿using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;

namespace MoveToCode {
    public class CloneOnDrag : MonoBehaviour {
        ManipulationHandler manipulationHandler;
        Vector3 startingPosition;
        GameObject clone;
        GameObject codeBlockType;
        bool stillInContactWithOriginal;
        bool blockStillInMenu;

        private MeshRenderer renderer;
        private MeshRenderer cloneRenderer;

        private void Awake() {
            startingPosition = transform.position;
            blockStillInMenu = true;
            renderer = GetComponent<MeshRenderer>();
        }

        public void SetCodeBlockType(GameObject cb) {
            codeBlockType = cb;
        }

        public void OnTriggerEnter() {
            stillInContactWithOriginal = true;
            if (cloneRenderer != null) {
                foreach (Material mat in cloneRenderer.materials) {
                    mat.SetFloat("_Outline", 0.15f);
                }
            }
        }

        public void OnTriggerExit() {
            stillInContactWithOriginal = false;
            if (cloneRenderer != null) {
                foreach (Material mat in cloneRenderer.materials) {
                    mat.SetFloat("_Outline", 0f);
                }
            }
            if (renderer != null) {
                foreach (Material mat in renderer.materials) {
                    mat.SetFloat("_Outline", 0f);
                }
            }
        }

        public void OnEnable() {
            manipulationHandler = GetComponent<ManipulationHandler>();
            manipulationHandler.OnManipulationStarted.AddListener(StartedMotion);
            manipulationHandler.OnManipulationEnded.AddListener(StoppedMotion);
        }

        private void StoppedMotion(ManipulationEventData arg0) {
            if (stillInContactWithOriginal) {
                Destroy(gameObject);
            } else {
                transform.SnapToCodeBlockManager();
                blockStillInMenu = false;
            }
            if (cloneRenderer != null) {
                foreach (Material mat in cloneRenderer.materials) {
                    mat.SetFloat("_Outline", 0f);
                }
            }
            if (renderer != null) {
                foreach (Material mat in renderer.materials) {
                    mat.SetFloat("_Outline", 0f);
                }
            }
        }

        private void StartedMotion(ManipulationEventData arg0) {
            if (blockStillInMenu) {
                clone = InstantiateBlock(codeBlockType, startingPosition);
                cloneRenderer = clone.GetComponent<MeshRenderer>();
                transform.SnapToCodeBlockManager();
                Destroy(transform.GetComponent<CloneOnDrag>());
            }
        }

        private GameObject InstantiateBlock(GameObject block, Vector3 spawnPos) {
            GameObject go = Instantiate(block, spawnPos, Quaternion.identity);
            go.AddComponent<CloneOnDrag>().SetCodeBlockType(codeBlockType);
            go.transform.SetParent(transform.parent);
            go.transform.localScale = Vector3.one;
            return go;
        }
    }
}