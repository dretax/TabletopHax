using System;
using Fougerite.Patcher;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Tabletopatcher.Patcher;

namespace Tabletopatcher
{
    internal class Program
    {
        private static AssemblyDefinition AssemblyCSharp = null;
        private static AssemblyDefinition DLCDreTaX = null;
        
        public static void Main(string[] args)
        {
            Console.WriteLine("CIL Patcher created by DreTaX. https://github.com/dretax");
            try
            {
                AssemblyCSharp = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
                DLCDreTaX = AssemblyDefinition.ReadAssembly("DLCDreTaX.dll");
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }

            try
            {
                TypeDefinition DLCManager = AssemblyCSharp.MainModule.GetType("DLCManager");
                TypeDefinition LoadDLCAssetBundleFromDisk =
                    DLCManager.GetNestedType("<LoadDLCAssetBundleFromDisk>c__Iterator2");
                LoadDLCAssetBundleFromDisk.IsPublic = true;
                LoadDLCAssetBundleFromDisk.IsSealed = true;
                foreach (var x in LoadDLCAssetBundleFromDisk.Fields)
                {
                    x.SetPublic(true);
                }

                foreach (var x in LoadDLCAssetBundleFromDisk.Properties)
                {
                    x.GetMethod.SetPublic(true);
                    if (x.SetMethod != null)
                    {
                        x.SetMethod.SetPublic(true);
                    }
                }

                foreach (var x in LoadDLCAssetBundleFromDisk.Methods)
                {
                    x.SetPublic(true);
                }

                foreach (var x in DLCManager.NestedTypes)
                {
                    Logger.Log(x.Name);
                }

                MethodDefinition LoadFake = DLCDreTaX.MainModule.GetType("DLCDreTaX.Library").GetMethod("LoadFake");
                MethodDefinition CanWeLoadThisDLCFake = DLCDreTaX.MainModule.GetType("DLCDreTaX.Library").GetMethod("CanWeLoadThisDLCFake");
                MethodDefinition GetOwnedDLCsFake = DLCDreTaX.MainModule.GetType("DLCDreTaX.Library").GetMethod("GetOwnedDLCsFake");
                
                
                MethodDefinition CanWeLoadThisDLC = DLCManager.GetMethod("CanWeLoadThisDLC");
                ILProcessor processor2 = CanWeLoadThisDLC.Body.GetILProcessor();
                processor2.Body.Instructions.Clear();
                processor2.Body.Instructions.Add(Instruction.Create(OpCodes.Ldarg_0));
                processor2.Body.Instructions.Add(Instruction.Create(OpCodes.Call, AssemblyCSharp.MainModule.Import(CanWeLoadThisDLCFake)));
                processor2.Body.Instructions.Add(Instruction.Create(OpCodes.Ret));
                
                MethodDefinition GetOwnedDLCs = DLCManager.GetMethod("GetOwnedDLCs");
                ILProcessor processor3 = GetOwnedDLCs.Body.GetILProcessor();
                processor3.Body.Instructions.Clear();
                processor3.Body.Instructions.Add(Instruction.Create(OpCodes.Call, AssemblyCSharp.MainModule.Import(GetOwnedDLCsFake)));
                processor3.Body.Instructions.Add(Instruction.Create(OpCodes.Ret));
                
                

                MethodDefinition Load = DLCManager.GetMethod("Load");
                ILProcessor processor = Load.Body.GetILProcessor();
                processor.Body.Instructions.Clear();
                processor.Body.Instructions.Add(Instruction.Create(OpCodes.Ldarg_0));
                processor.Body.Instructions.Add(Instruction.Create(OpCodes.Ldarg_1));
                processor.Body.Instructions.Add(Instruction.Create(OpCodes.Call, AssemblyCSharp.MainModule.Import(LoadFake)));
                processor.Body.Instructions.Add(Instruction.Create(OpCodes.Ret));
            }
            catch (Exception ex)
            {
                Logger.Log(ex.ToString());
            }
            

            // <LoadDLCAssetBundleFromDisk>c__Iterator2 
            //MethodDefinition md = DLCManager.GetMethod("MoveNext");
            
            try
            {

                AssemblyCSharp.Write("Assembly-CSharp.dll");
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }
            Console.WriteLine("Patching complete!");
            Console.ReadKey();
        }
    }
}