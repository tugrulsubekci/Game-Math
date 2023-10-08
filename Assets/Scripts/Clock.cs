using System;
using UnityEditor;
using UnityEngine;

public class Clock : MonoBehaviour
{
    private float Hour
    {
        get {
            var currentHour = DateTime.Now.Hour + (smoothHour ? Minute / MathUtils.totalMinutes : 0);
            return currentHour % TotalHours == 0 ? TotalHours : currentHour % TotalHours;
        }
    }
    private float Minute {
        get {
            var currentMinute = DateTime.Now.Minute + (smoothMinute ? Second / MathUtils.totalSeconds : 0);
            return currentMinute;
        }
    }
    private float Second
    {
        get {
            var currentSecond = DateTime.Now.Second + (smoothSecond ? DateTime.Now.Millisecond / MathUtils.totalMiliseconds : 0);
            return currentSecond;
        }
    }

    private float TotalHours
    {
        get {
            return hours24 ? MathUtils.totalHours * 2.0f : MathUtils.totalHours;
        }
    }

    [SerializeField] private bool smoothHour;
    [SerializeField] private bool smoothMinute;
    [SerializeField] private bool smoothSecond;
    [SerializeField] private bool hours24;

    [Header("Gizmo Settings")]
    [Range(0,1)]
    public float hourLength = 0.5f;
    [Range(0,1)]
    public float minuteLength = 0.7f;
    [Range(0,1)]
    public float secondLength = 0.9f;
    [Range(1,10)]
    public float hourTickness = 6.0f;
    [Range(1,10)]
    public float minuteTickness = 3f;
    [Range(1,10)]
    public float secondTickness = 1f;
    private void OnDrawGizmos() {
        Handles.Disc(transform.rotation, transform.position, transform.forward, 1.0f, false, 0f);

        var hourAngle = MathUtils.roundAngle / TotalHours * Hour;
        var minuteAngle = MathUtils.roundAngle / MathUtils.totalMinutes * Minute;
        var secondAngle = MathUtils.roundAngle / MathUtils.totalSeconds * Second;

        var hourDirection = MathUtils.AngleToDirection(hourAngle);
        var minuteDirection = MathUtils.AngleToDirection(minuteAngle);
        var secondDirection = MathUtils.AngleToDirection(secondAngle);

        var hourPosition = hourDirection * hourLength + transform.position;
        var minutePosition = minuteDirection * minuteLength + transform.position;
        var secondPosition = secondDirection * secondLength + transform.position;

        Handles.DrawLine(transform.position, hourPosition, hourTickness);
        Handles.DrawLine(transform.position, minutePosition, minuteTickness);
        Handles.DrawLine(transform.position, secondPosition, secondTickness);

        for (int i = 0; i < TotalHours; i++)
            Handles.DrawLine
            (
                MathUtils.AngleToDirection(MathUtils.roundAngle / TotalHours * i) * 0.9f + transform.position,
                MathUtils.AngleToDirection(MathUtils.roundAngle / TotalHours * i) * 1.1f + transform.position,
                hourTickness
            );


        for (int i = 0; i < MathUtils.totalMinutes; i++)
            Handles.DrawLine
            (
                MathUtils.AngleToDirection(MathUtils.roundAngle / MathUtils.totalMinutes * i) * 0.95f + transform.position,
                MathUtils.AngleToDirection(MathUtils.roundAngle / MathUtils.totalMinutes * i) * 1.05f + transform.position,
                minuteTickness
            );
    }
}
