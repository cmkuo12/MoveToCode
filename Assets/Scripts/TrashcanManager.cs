﻿using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using UnityEngine;

namespace MoveToCode {
    public class TrashcanManager : Singleton<TrashcanManager> {
        static string trashcanCol = "deletedObj";
        public AnimationCurve animCurve;
        public Vector3 shakeMagnitude;
        MeshOutline meshOutline;
        CodeBlock codeBlockInCollision;
        GameObject particleSystemDummyObject;
        bool currentlyColliding = false;

        void Awake() {
            meshOutline = GetComponent<MeshOutline>();
            particleSystemDummyObject = transform.GetChild(0).gameObject;
            LoggingManager.instance.AddLogColumn(trashcanCol, "");
        }

        void OnTriggerEnter(Collider other) {
            codeBlockInCollision = other.gameObject.GetDestructableCodeBlockObject();
            if (codeBlockInCollision != null) {
                CodeBlockCollided();
            }
        }
        void OnTriggerExit(Collider other) {
            currentlyColliding = false;
            meshOutline.enabled = false;
            codeBlockInCollision?.GetComponent<ManipulationHandler>().OnManipulationEnded.RemoveListener(CheckIfStillInCollision);
            codeBlockInCollision = null;
        }

        void CodeBlockCollided() {
            currentlyColliding = true;
            meshOutline.enabled = true;
            particleSystemDummyObject.transform.position = codeBlockInCollision.gameObject.GetComponent<Transform>().position;
            codeBlockInCollision.GetComponent<ManipulationHandler>().OnManipulationEnded.AddListener(CheckIfStillInCollision);
        }

        private void CheckIfStillInCollision(ManipulationEventData arg0) {
            if (currentlyColliding) {
                AudioManager.instance.PlaySoundAtObject(gameObject, AudioManager.poofAudioClip);
                LoggingManager.instance.UpdateLogColumn(trashcanCol, codeBlockInCollision.name);
                StartCoroutine(Shake());
                ParticleSystem ps = particleSystemDummyObject.GetComponent<ParticleSystem>();
                ps.GetComponent<ParticleSystemRenderer>().material = codeBlockInCollision.GetCodeBlockMaterial();
                ps.Play();
                codeBlockInCollision.GetComponent<ManipulationHandler>().OnManipulationEnded.RemoveListener(CheckIfStillInCollision);
                codeBlockInCollision.gameObject.SetActive(false);
            }
            currentlyColliding = false;
            codeBlockInCollision = null;
        }

        IEnumerator Shake() {
            Vector3 startPos = transform.position, leftPos = startPos - shakeMagnitude, rightPos = startPos + shakeMagnitude;
            float curTime = 0f, totalTime = 0.4f;
            while (curTime < totalTime) {
                transform.position = Vector3.Lerp(leftPos, rightPos, animCurve.Evaluate(curTime / totalTime));
                yield return new WaitForSeconds(Time.deltaTime);
                curTime += Time.deltaTime;
            }
            transform.position = startPos;
        }

    }
}
