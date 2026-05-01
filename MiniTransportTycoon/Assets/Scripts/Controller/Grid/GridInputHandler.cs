#nullable enable
using System;

namespace Controller.Grid
{
    public class GridInputHandler
    {
        private readonly IBuildSelectionManager _buildSelectionManager;
        private readonly GridBuildService _gridBuildService;
        private readonly GridDemolishService _gridDemolishService;
        private readonly Func<bool> _isRouteCreationActive;

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
            gameInput.OnDeleteKeyPressed += GameInputOnDeleteKeyPressed;
        }

        public void Unbind(GameInput gameInput)
        {
            gameInput.OnDeleteKeyPressed -= GameInputOnDeleteKeyPressed;
            gameInput.OnLeftClickPressed -= GameInputOnLeftClickPressed;
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
