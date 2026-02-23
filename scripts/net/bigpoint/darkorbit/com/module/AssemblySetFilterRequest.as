package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class AssemblySetFilterRequest implements IModule
   {
      private static var _instance:AssemblySetFilterRequest;
      
      public static const FILTERTYPE_ASSEMBLY:int = 0;
      
      public static const FILTERTYPE_UPGRADE:int = 1;
      
      public static const FILTERTYPE_DISASSEMBLY:int = 2;
      
      public static const FILTERTYPE__MAX:int = 3;
      
      public var filterType:int = 0;
      
      public var assemblyWindowSettingsModule:AssemblyWindowSettingsModule;
      
      public function AssemblySetFilterRequest(param1:AssemblyWindowSettingsModule = null, param2:int = 0)
      {
         super();
         this.filterType = param2;
         if(param1 == null)
         {
            this.assemblyWindowSettingsModule = new AssemblyWindowSettingsModule();
         }
         else
         {
            this.assemblyWindowSettingsModule = param1;
         }
      }
      
      public static function get instance() : AssemblySetFilterRequest
      {
         return _instance || (_instance = new AssemblySetFilterRequest());
      }
      
      public function getLibcomModuleId() : int
      {
         return -32215;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 0;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.filterType = param1.readShort();
         this.assemblyWindowSettingsModule = AssemblyWindowSettingsModule(ModuleFactory.get().createInstance(param1.readShort()));
         if(null != this.assemblyWindowSettingsModule)
         {
            this.assemblyWindowSettingsModule.read(param1);
         }
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(-32215);
         param1.writeShort(this.filterType);
         if(null != this.assemblyWindowSettingsModule)
         {
            this.assemblyWindowSettingsModule.write(param1);
         }
         else
         {
            param1.writeShort(0);
         }
      }
   }
}

