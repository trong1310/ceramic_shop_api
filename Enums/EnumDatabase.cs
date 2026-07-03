using System.ComponentModel;

namespace CeramicsShopMasterApi.Enums
{
    public enum edIsEnable
    {
        DISABLE,
        ENABLE
    }

    public enum edState
    {
        LOCKED,
        ACTIVE
    }
    public enum edOrderState
    {
        /// <summary>
        /// Chờ thanh toán
        /// </summary>
        Pending = 1,

        /// <summary>
        /// Đã thanh toán
        /// </summary>
        Paid = 2

    }
}
