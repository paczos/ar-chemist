//-----------------------------------------------------------------------
// <copyright file="AugmentedImageExampleController.cs" company="Google">
//
// Copyright 2018 Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------

namespace GoogleARCore.Examples.AugmentedImage
{
    using GoogleARCore;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(AudioSource))]
    public class AugmentedImageExampleController : MonoBehaviour
    {

 
        /// <summary>
        /// A prefab for visualizing an AugmentedImage.
        /// </summary>
        public Button ClearButton;
        /// 
        public AugmentedImageVisualizer AtomVisualizerPrefab;
        public GameObject CurrentMoleculeLabel;
        public AudioSource successfulDetectionSound;
        /// <summary>
        /// The overlay containing the fit to scan user guide.
        /// </summary>
        public GameObject FitToScanOverlay;

        public Dictionary<int, AugmentedImageVisualizer> Visualizers
            = new Dictionary<int, AugmentedImageVisualizer>();

        private readonly List<AugmentedImage> m_TempAugmentedImages = new List<AugmentedImage>();



        public void ResetCurrentMolecule()
        {
            Visualizers.Clear();
            ClearButton.gameObject.SetActive(false);

        }
        /// <summary>
        /// The Unity Update method.
        /// </summary>
        public void Update()
        {
            // Exit the app when the 'back' button is pressed.
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }

            // Check that motion tracking is tracking.
            if (Session.Status != SessionStatus.Tracking)
            {
                return;
            }

            // Get updated augmented images for this frame.
            Session.GetTrackables(m_TempAugmentedImages, TrackableQueryFilter.Updated);

            // Create visualizers and anchors for updated augmented images that are tracking and do not previously
            // have a visualizer. Remove visualizers for stopped images.
            foreach (var image in m_TempAugmentedImages)
            {
                AugmentedImageVisualizer visualizer = null;
                Visualizers.TryGetValue(image.DatabaseIndex, out visualizer);
                if (image.TrackingState == TrackingState.Tracking && visualizer == null)
                {
                    var audioData = GetComponent<AudioSource>();
                    audioData.Play(0);

                    // Create an anchor to ensure that ARCore keeps tracking this augmented image.
                    Anchor anchor = image.CreateAnchor(image.CenterPose);
                    visualizer = Instantiate(AtomVisualizerPrefab, anchor.transform);
                    visualizer.Image = image;
                    Visualizers.Add(image.DatabaseIndex, visualizer);

                }
                else if (image.TrackingState == TrackingState.Stopped && visualizer != null)
                {
                    Visualizers.Remove(image.DatabaseIndex);
                    GameObject.Destroy(visualizer.gameObject);
                }
            }
            // Show the fit-to-scan overlay if there are no images that are Tracking.
            CurrentMoleculeLabel.GetComponent<Text>().text = string.Join(" ",
                Visualizers.Values
                .Select(m => m.Image.Name)
                .ToArray());

            if (Visualizers.Count != 0)
            {
                ClearButton.gameObject.SetActive(true);
            }
            foreach (var visualizer in Visualizers.Values)
            {
                if (visualizer.Image.TrackingState == TrackingState.Tracking)
                {
                    FitToScanOverlay.SetActive(false);
                    return;
                }
            }

            FitToScanOverlay.SetActive(true);
        }
    }
}
