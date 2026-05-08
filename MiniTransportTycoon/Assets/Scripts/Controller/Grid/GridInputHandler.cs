#nullable enable
using System;
using UnityEngine.InputSystem;

namespace Controller.Grid
{
    public class GridInputHandler
    {
        private readonly IBuildSelectionManager _buildSelectionManager;
        private readonly GridBuildService _gridBuildService;
        private readonly GridDemolishService _gridDemolishService;
        private readonly Func<bool> _isRouteCreationActive;
        private Location? _lastDemolishedLocation;

        public GridInputHandler(
            IBuildSelectionManager buildSelectionManager,
            GridBuildService gridBuildService,
            GridDemolishService gridDemolishService,
            Func<bool> isRouteCreationActive)
        {
            _buildSelectionManager = buildSelectionManager;
            _gridBuildService = gridBuildService;
            _gridDemolishService = gridDemolishService;
            _isRouteCreationActive = isRouteCreationActive;
        }

        public void Bind(GameInput gameInput)
        {
            gameInput.OnLeftClickPressed += GameInputOnLeftClickPressed;
        }

        public void Unbind(GameInput gameInput)
        {
            gameInput.OnLeftClickPressed -= GameInputOnLeftClickPressed;
        }

        public void HandleHeldInput()
        {
            Keyboard? keyboard = Keyboard.current;
            if (keyboard == null)
            {
                _lastDemolishedLocation = null;
                return;
            }

            if (!keyboard.deleteKey.isPressed)
            {
                _lastDemolishedLocation = null;
                return;
            }

            if (!GridManager.Instance!.TryGetMouseGridLocation(out Location location)) return;
            if (_lastDemolishedLocation == location) return;

            GameInputOnDeleteKeyPressed(this, EventArgs.Empty);
            _lastDemolishedLocation = location;
        }

        private void GameInputOnLeftClickPressed(object? sender, EventArgs e)
        {
            if (Utils.IsPointerOverBlockingUI()) return;
            if (_isRouteCreationActive()) return;
            if (_buildSelectionManager.SelectedObjectType is null) return;

            _gridBuildService.BuildOnCurrentMousePosition();
        }

        private void GameInputOnDeleteKeyPressed(object? sender, EventArgs e)
        {
            if (Utils.IsPointerOverBlockingUI()) return;
            if (_isRouteCreationActive()) return;

            _gridDemolishService.DemolishAtCurrentMousePosition();
        }
    }
}
