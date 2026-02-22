package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class AssemblyRecipeKeyModule implements IModule
   {
      private static var _instance:AssemblyRecipeKeyModule;
      
      public var targetId:Number = 0;
      
      public var lootId:String = "";
      
      public function AssemblyRecipeKeyModule(param1:String = "", param2:Number = 0)
      {
         super();
         this.targetId = param2;
         this.lootId = param1;
      }
      
      public static function get instance() : AssemblyRecipeKeyModule
      {
         return _instance || (_instance = new AssemblyRecipeKeyModule());
      }
      
      public function getLibcomModuleId() : int
      {
         return -2259;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 10;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.targetId = param1.readDouble();
         this.lootId = param1.readUTF();
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(-2259);
         param1.writeDouble(this.targetId);
         param1.writeUTF(this.lootId);
      }
   }
}

