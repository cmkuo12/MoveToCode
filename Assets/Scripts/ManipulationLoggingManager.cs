﻿using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
namespace MoveToCode {
    public class ManipulationLoggingManager : Singleton<ManipulationLoggingManager> {
        static string manipColName = "ManipulatingObject";

        private void Start() {
            LoggingManager.instance.AddLogColumn(manipColName, "");
            StartCoroutine(AddLoggersToManipulationHandlers());
        }

        IEnumerator AddLoggersToManipulationHandlers() {
            yield return null;
            foreach (var go in FindObjectsOfType<ManipulationHandler>()) {
                go.gameObject.AddComponent<ManipulationLogger>();
            }
            foreach (var go in FindObjectsOfType<PressableButtonHoloLens2>()) {
                go.gameObject.AddComponent<ManipulationLogger>();
            }
        }

        public static string GetColName() {
            return manipColName;
        }

    }
}