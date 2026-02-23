package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class EternalBlacklightRankModule implements IModule
   {
      private static var _instance:EternalBlacklightRankModule;
      
      public var rank:int = 0;
      
      public var lastUpdateTime:String = "";
      
      public var name:String = "";
      
      public var waves:int = 0;
      
      public function EternalBlacklightRankModule(param1:int = 0, param2:String = "", param3:String = "", param4:int = 0)
      {
         super();
         this.rank = param1;
         this.lastUpdateTime = param3;
         this.name = param2;
         this.waves = param4;
      }
      
      public static function get instance() : EternalBlacklightRankModule
      {
         return _instance || (_instance = new EternalBlacklightRankModule());
      }
      
      public function getLibcomModuleId() : int
      {
         return -7406;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 12;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.rank = param1.readInt();
         this.rank = this.rank << 10 | this.rank >>> 22;
         this.lastUpdateTime = param1.readUTF();
         this.name = param1.readUTF();
         this.waves = param1.readInt();
         this.waves = this.waves << 15 | this.waves >>> 17;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(-7406);
         param1.writeInt(this.rank >>> 10 | this.rank << 22);
         param1.writeUTF(this.lastUpdateTime);
         param1.writeUTF(this.name);
         param1.writeInt(this.waves >>> 15 | this.waves << 17);
      }
   }
}

