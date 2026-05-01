#nullable enable
using System;

namespace Controller.Grid
{
    public class GridInputHandler
    {
        private readonly GridBuildService _gridBuildService;
        private readonly GridDemolishService _gridDemolishService;

        public GridInputHandler(
            GridBuildService gridBuildService,
            GridDemolishService gridDemolishService)
        {
            _gridBuildService = gridBuildService;
            _gridDemolishService = gridDemolishService;
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
            if (RouteCreationManager.Instance.InRouteCreation) return;
            if (BuildSelectionManager.Instance.SelectedObjectType is null) return;

            _gridBuildService.BuildOnCurrentMousePosition();
        }

        private void GameInputOnDeleteKeyPressed(object? sender, EventArgs e)
        {
            if (Utils.IsPointerOverBlockingUI()) return;
            if (RouteCreationManager.Instance.InRouteCreation) return;

            _gridDemolishService.DemolishAtCurrentMousePosition();
        }
    }
}
