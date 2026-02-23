package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class UnstableModuleRecycleResultCommand implements IModule
   {
      private static var _instance:UnstableModuleRecycleResultCommand;
      
      public var shardCommand:UnstableModulesShardCommand;
      
      public var itemId:int = 0;
      
      public function UnstableModuleRecycleResultCommand(param1:int = 0, param2:UnstableModulesShardCommand = null)
      {
         super();
         if(param2 == null)
         {
            this.shardCommand = new UnstableModulesShardCommand();
         }
         else
         {
            this.shardCommand = param2;
         }
         this.itemId = param1;
      }
      
      public static function get instance() : UnstableModuleRecycleResultCommand
      {
         return _instance || (_instance = new UnstableModuleRecycleResultCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return -24446;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 4;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.shardCommand = UnstableModulesShardCommand(ModuleFactory.get().createInstance(param1.readShort()));
         if(null != this.shardCommand)
         {
            this.shardCommand.read(param1);
         }
         this.itemId = param1.readInt();
         this.itemId = this.itemId >>> 16 | this.itemId << 16;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(-24446);
         if(null != this.shardCommand)
         {
            this.shardCommand.write(param1);
         }
         else
         {
            param1.writeShort(0);
         }
         param1.writeInt(this.itemId << 16 | this.itemId >>> 16);
      }
   }
}

