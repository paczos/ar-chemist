//-----------------------------------------------------------------------
// <copyright file="AugmentedImageVisualizer.cs" company="Google">
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
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Uses 4 frame corner objects to visualize an AugmentedImage.
    /// </summary>
    public class AugmentedImageVisualizer : MonoBehaviour
    {
        /// <summary>
        /// The AugmentedImage to visualize.
        /// </summary>
        public AugmentedImage Image;

        public GameObject AtomRepresentation;
        /// <summary>
        /// The Unity Update method.
        /// </summary>
        public void Update()
        {
            if (Image == null || Image.TrackingState != TrackingState.Tracking)
            {
                AtomRepresentation.SetActive(false);
                return;
            }
            //float halfWidth = Image.ExtentX / 2;
            //float halfHeight = Image.ExtentZ / 2;
            AtomRepresentation.transform.SetPositionAndRotation(
                new Vector3(Image.ExtentX, 
                AtomRepresentation.transform.position.y,
                Image.ExtentZ), 
                Quaternion.identity);
            //FrameLowerLeft.transform.localPosition = (halfWidth * Vector3.left) + (halfHeight * Vector3.back);
            //FrameLowerRight.transform.localPosition = (halfWidth * Vector3.right) + (halfHeight * Vector3.back);
            //FrameUpperLeft.transform.localPosition = (halfWidth * Vector3.left) + (halfHeight * Vector3.forward);
            //FrameUpperRight.transform.localPosition = (halfWidth * Vector3.right) + (halfHeight * Vector3.forward);

            //FrameLowerLeft.SetActive(true);
            //FrameLowerRight.SetActive(true);
            //FrameUpperLeft.SetActive(true); 
            //FrameUpperRight.SetActive(true);
        }
    }
}
