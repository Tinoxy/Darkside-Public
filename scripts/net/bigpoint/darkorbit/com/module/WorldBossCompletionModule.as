package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class WorldBossCompletionModule implements IModule
   {
      private static var _instance:WorldBossCompletionModule;
      
      public var completionTo:int = 0;
      
      public var completionFrom:int = 0;
      
      public var normalAmount:int = 0;
      
      public var easyAmount:int = 0;
      
      public var lootId:String = "";
      
      public function WorldBossCompletionModule(param1:String = "", param2:int = 0, param3:int = 0, param4:int = 0, param5:int = 0)
      {
         super();
         this.completionTo = param3;
         this.completionFrom = param2;
         this.normalAmount = param5;
         this.easyAmount = param4;
         this.lootId = param1;
      }
      
      public static function get instance() : WorldBossCompletionModule
      {
         return _instance || (_instance = new WorldBossCompletionModule());
      }
      
      public function getLibcomModuleId() : int
      {
         return -26487;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 18;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.completionTo = param1.readInt();
         this.completionTo = this.completionTo >>> 7 | this.completionTo << 25;
         this.completionFrom = param1.readInt();
         this.completionFrom = this.completionFrom >>> 16 | this.completionFrom << 16;
         this.normalAmount = param1.readInt();
         this.normalAmount = this.normalAmount >>> 11 | this.normalAmount << 21;
         this.easyAmount = param1.readInt();
         this.easyAmount = this.easyAmount << 13 | this.easyAmount >>> 19;
         this.lootId = param1.readUTF();
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(-26487);
         param1.writeInt(this.completionTo << 7 | this.completionTo >>> 25);
         param1.writeInt(this.completionFrom << 16 | this.completionFrom >>> 16);
         param1.writeInt(this.normalAmount << 11 | this.normalAmount >>> 21);
         param1.writeInt(this.easyAmount >>> 13 | this.easyAmount << 19);
         param1.writeUTF(this.lootId);
      }
   }
}

