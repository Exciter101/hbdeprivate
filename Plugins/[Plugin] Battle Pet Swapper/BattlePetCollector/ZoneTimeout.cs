using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattlePetCollector
{
    public class ZoneTimeout : IZoneTimeout
    {
        private IPluginProperties _pluginProperties;
        private IPluginLogger _logger;

        public ZoneTimeout(IPluginLogger logger, IPluginProperties pluginProperties)
        {
            _logger = logger;
            _pluginProperties = pluginProperties;
        }

        private List<string> _visitedZones = new List<string>();

        public bool HasVistedZone(Zone zone)
        {
            return _visitedZones.Contains(zone.Name);
        }

        public bool HasVistedZones
        {
            get
            {
                return _visitedZones.Count > 0;
            }
        }

        public void ClearVisitedZones()
        {
            _visitedZones = new List<string>(); ;
        }

        Zone _targetZone;
        Zone _lastZoneVisited;
        DateTime _zoneEntryTime;
        bool _inZone = false;

        public void SetTargetZone(Zone zone)
        {
            _logger.Write("Target zone set to " + zone);
            _targetZone = zone;
            _inZone = false;
        }

        public bool ZoneTimeoutReached(Zone currentZone)
        {
            UpdateCurrentZoneEntry(currentZone);

            bool result = MinutesInZoneLeft(currentZone) < 0;
            if (result) 
            { 
                _logger.Write("Zone timeout reached...switching zone.");
                _visitedZones.Add(currentZone.Name);
            }
            return result;
        }

        private int MinutesInZoneLeft(Zone currentZone)
        {
            return _pluginProperties.MaxMinsPerZone - MinutesInZone(currentZone);
        }

        public int MinutesInZone(Zone currentZone)
        {
            if (_targetZone == null) { return 0; }

            UpdateCurrentZoneEntry(currentZone);

            if (currentZone.Name != _targetZone.Name) { return _pluginProperties.MaxMinsPerZone; }
            return (int)(DateTime.Now - _zoneEntryTime).TotalMinutes;
        }

        public void LogTimeInZone(Zone currentZone)
        {
            if (_targetZone==null) { return; }

            UpdateCurrentZoneEntry(currentZone);
            if (_inZone)
            {
                _logger.Write("In zone " + _targetZone + " we have " + MinutesInZoneLeft(currentZone) + " minutes left to trap pets.");
            }
            else
            {
                _logger.Write("Waiting to reach target zone " + _targetZone + " (from " + currentZone+")");
            }
        }

        public void UpdateCurrentZoneEntry(Zone currentZone)
        {
            if (_targetZone != null && currentZone.Name == _targetZone.Name && (_lastZoneVisited==null || _lastZoneVisited.Name != currentZone.Name))
            {
                _logger.Write("Entry to " + _targetZone + " detected.");
                _lastZoneVisited = currentZone;
                _inZone = true;
                _zoneEntryTime = DateTime.Now;
            }
        }

        public void ResetZoneTimeout()
        {
            _logger.Write("Caught a new pet, zone timeout reset.");
            _zoneEntryTime = DateTime.Now;
        }
    }
}
