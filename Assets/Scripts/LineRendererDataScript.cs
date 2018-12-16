using System;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineRendererDataScript : MonoBehaviour {

    public int NumberOfPoints = 40;

    private float[] _points;

    private int     _currentIndex;

    private LineRenderer _renderer;

	// Use this for initialization
	void Start () {
        //Initialize buffers
        _points = new float[NumberOfPoints];
        //Set the current index to add a new point into the renderer line
        _currentIndex = (int)(NumberOfPoints / 1.3);
        _renderer = GetComponent<LineRenderer>();

        //Set the number of points for the renderer line
        _renderer.positionCount = NumberOfPoints;
	}
	
    public void AddPoint(float value) {
        //Create a copy of the old array to shift values to the left
        var newArray = new float[NumberOfPoints];

        Array.Copy(_points, 1, newArray, 0, _currentIndex);
        //****  FASTEST WAY TO ACHIEVE THIS : https://stackoverflow.com/questions/2381245/c-sharp-quickest-way-to-shift-array

        _points = newArray;
        _points[_currentIndex] = Mathf.Clamp(value, -2, 2);

        for (var i = 0; i < NumberOfPoints; ++i) {
            var z       = -_points[i] * GetCoeff(i);

            //Add a fake value after the current index to decrease the "gap effect" at the current index
            if (i > _currentIndex) {
                z = (-_points[_currentIndex] * GetCoeff(_currentIndex)) / (float)Math.Pow((i - _currentIndex), 1.2);
            }
            //Set a vector for the new point
            Vector3 vector = new Vector3(Mathf.Abs(i * (_renderer.transform.position.x * 2.0f) / NumberOfPoints), 0, z);

            _renderer.SetPosition(i, vector);
        }
    }

    private float GetCoeff(int index) {

        //var value = 1.0f;

        //Return a value from a function to create a curve : https://www.wolframalpha.com/input/?i=interpolate+%5B(0,+0.05),+(20,+0.05)+(40,+1),+(50,+1)+,+(60,+1),+(80,+0.05),+(100,+0.05)%5D
        var value = 7.25694 * Mathf.Pow(10, -10) * Mathf.Pow(index, 6) - 2.17708 * Mathf.Pow(10, -7) * Mathf.Pow(index, 5) + 0.0000251684 * Mathf.Pow(index, 4) - 0.00140521 * Mathf.Pow(index, 3) + 0.037554 * Mathf.Pow(index, 2) - 0.357833 * index + 0.05;

        return Mathf.Clamp((float)value, 0.05f, 1.0f);

    }
}
