using BlazorApp1.Modals;
using Microsoft.JSInterop;

namespace ShinySparkle_Server.Components.Pages.ShinySparkle
{
    public partial class ProductList
    {
        private IEnumerable<ProductVM> products { get; set; } = new List<ProductVM>();  //Model Binding.....
        private int? DeleteRoomId { get; set; } = null;
        private bool IsProcessing { get; set; } = false;


        protected override async Task OnInitializedAsync()
        {
            products = await ProductListRepository.GetAllProduct();
        }
        //private async Task HandleDelete(int roomId)
        //{
        //    DeleteRoomId = roomId;
        //    await JSRuntime.InvokeVoidAsync("ShowDeleteConfirmationModal");
        //}
        //public async Task ConfirmDelete_Click(bool isConfirmed)
        //{
        //    IsProcessing = true;
        //    if (isConfirmed && DeleteRoomId != null)
        //    {
        //        HotelRoomDTO obj = await Hotelroomreporsitory.GetHotelRoom(DeleteRoomId.Value);
        //        foreach (var i in obj.HotelRoomImages)
        //        {
        //            var imageName = i.RoomImageUrl.Replace($"RoomImages/", "");
        //            FileUpload.DeleteFile(imageName);
        //        }
        //        await Hotelroomreporsitory.DeleteHotelRoom(DeleteRoomId.Value);
        //        //await JSRuntime.InvokeVoidAsync("ToastrSuccess", "Hotel Room Deleted successfully");
        //        await JSRuntime.InvokeVoidAsync("HideDeleteConfirmationModal");
        //        hotelRooms = await Hotelroomreporsitory.GetAllHotelRoom();
        //        IsProcessing = false;
        //    }

        //}







    }
}