using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class PreviewTrackCameraController : MonoBehaviour
{
    public float rotationSpeed;
    public Image previewTrackImage;
    public TrackBuilder previewTrack;

    private Vector3 trackCentroid;
    private Vector2 rotationAngles;
    private bool mouseDown = false;
    private bool mouseOverPreviewImage = false;
    private bool firstFrame = true;

	void Start ()
    {
	}
	
	void Update () 
    {
        if (firstFrame)
        {
            firstFrame = false;
            CenterCameraOnTrack();
        }

        if (mouseOverPreviewImage && (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)))
        {
            mouseDown = true;
        }

        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        {
            mouseDown = false;
        }

        if (mouseDown)
        {
            transform.RotateAround(trackCentroid, transform.right, rotationSpeed * -Input.GetAxis("Mouse Y"));
            transform.RotateAround(trackCentroid, transform.up, rotationSpeed * Input.GetAxis("Mouse X"));
        }
	}

    public void OnMouseEntersPreviewTrackImage()
    {
        mouseOverPreviewImage = true;
    }

    public void OnMouseExitsPreviewTrackImage()
    {
        mouseOverPreviewImage = false;
    }

    public void CenterCameraOnTrack()
    {
        trackCentroid = Vector3.zero;
        float minX = Mathf.Infinity, maxX = Mathf.NegativeInfinity, 
              minZ = Mathf.Infinity, maxZ = Mathf.NegativeInfinity;

        foreach (TrackPiece trackPiece in previewTrack.GetTrackPieces())
        {
            trackCentroid += trackPiece.transform.position;
            MeshRenderer[] childrenMeshRenderers = trackPiece.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer mr in childrenMeshRenderers)
            {
                minX = Mathf.Min(minX, mr.bounds.center.x - mr.bounds.extents.x);
                maxX = Mathf.Max(maxX, mr.bounds.center.x + mr.bounds.extents.x);
                minZ = Mathf.Min(minZ, mr.bounds.center.z - mr.bounds.extents.z);
                maxZ = Mathf.Max(maxZ, mr.bounds.center.z + mr.bounds.extents.z);
            }
        }

        float sizeZ = (maxZ - minZ) / 2.0f;
        float sizeX = (maxX - minX) / 2.0f;
        GetComponent<Camera>().orthographicSize = Mathf.Max(sizeX, sizeZ) * 1.5f;

        trackCentroid /= previewTrack.GetTrackPieces().Count;
        transform.position = trackCentroid;
        transform.position += Vector3.up * 999.9f;
        transform.LookAt(trackCentroid);
    }
}
