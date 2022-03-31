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
                TypeDefinition PlayerManager = AssemblyCSharp.MainModule.GetType("PlayerManager");
                TypeDefinition Chat = AssemblyCSharp.MainModule.GetType("Chat");
                TypeDefinition UIGridMenuGames = AssemblyCSharp.MainModule.GetType("UIGridMenuGames");
                TypeDefinition SteamManager = AssemblyCSharp.MainModule.GetType("SteamManager");
                
                
                foreach (var x in PlayerManager.GetMethods())
                {
                    x.SetPublic(true);
                }
                
                foreach (var x in PlayerManager.Fields)
                {
                    x.SetPublic(true);
                }

                Chat.GetMethod("RPC_ChatMessage").SetPublic(true);
                /*TypeDefinition LoadDLCAssetBundleFromDisk =
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
                }*/

                foreach (var x in DLCManager.Methods)
                {
                    x.SetPublic(true);
                }

                foreach (var x in DLCManager.Fields)
                {
                    x.SetPublic(true);
                }
                
                MethodDefinition LoadFake = DLCDreTaX.MainModule.GetType("DLCDreTaX.Library").GetMethod("LoadFake");
                MethodDefinition CanWeLoadThisDLCFake = DLCDreTaX.MainModule.GetType("DLCDreTaX.Library").GetMethod("CanWeLoadThisDLCFake");
                MethodDefinition GetOwnedDLCsFake = DLCDreTaX.MainModule.GetType("DLCDreTaX.Library").GetMethod("GetOwnedDLCsFake");
                MethodDefinition GetDLCGridButtonsFake = DLCDreTaX.MainModule.GetType("DLCDreTaX.Library").GetMethod("GetDLCGridButtonsFake");
                MethodDefinition DLCStartFake = DLCDreTaX.MainModule.GetType("DLCDreTaX.Library").GetMethod("DLCStartFake");
                MethodDefinition SetKickstarterFlagsFake = DLCDreTaX.MainModule.GetType("DLCDreTaX.Library").GetMethod("SetKickstarterFlags");

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
                
                MethodDefinition Start = DLCManager.GetMethod("Start");
                ILProcessor processor5 = Start.Body.GetILProcessor();
                processor5.Body.Instructions.Clear();
                processor5.Body.Instructions.Add(Instruction.Create(OpCodes.Call, AssemblyCSharp.MainModule.Import(DLCStartFake)));
                processor5.Body.Instructions.Add(Instruction.Create(OpCodes.Ret));
                
                MethodDefinition GetDLCGridButtons = UIGridMenuGames.GetMethod("GetDLCGridButtons");
                ILProcessor processor4 = GetDLCGridButtons.Body.GetILProcessor();
                processor4.Body.Instructions.Clear();
                processor4.Body.Instructions.Add(Instruction.Create(OpCodes.Ldarg_0));
                processor4.Body.Instructions.Add(Instruction.Create(OpCodes.Ldarg_1));
                processor4.Body.Instructions.Add(Instruction.Create(OpCodes.Call, AssemblyCSharp.MainModule.Import(GetDLCGridButtonsFake)));
                processor4.Body.Instructions.Add(Instruction.Create(OpCodes.Ret));
                
                MethodDefinition Init = SteamManager.GetMethod("Init");
                int i = Init.Body.Instructions.Count - 10;
                ILProcessor iLProcessor = Init.Body.GetILProcessor();
                iLProcessor.InsertBefore(Init.Body.Instructions[i], Instruction.Create(OpCodes.Call, AssemblyCSharp.MainModule.Import(SetKickstarterFlagsFake)));
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