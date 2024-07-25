using UnityEngine;

namespace KeroTools.URPPlus.Runtime.Profiles
{
    public interface IProfile
    {
        void SetMaterialProfile(Material material);
        void AddChildMaterial(Material material);
        void RemoveChildMaterial(Material material);
    }
}