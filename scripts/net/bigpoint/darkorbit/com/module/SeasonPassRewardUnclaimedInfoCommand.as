package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class SeasonPassRewardUnclaimedInfoCommand implements IModule
   {
      private static var _instance:SeasonPassRewardUnclaimedInfoCommand;
      
      public var marketingImage:String = "";
      
      public var captainWarning:Boolean = false;
      
      public var captainOwned:Number = 0;
      
      public var seasonName:String = "";
      
      public var captainPending:int = 0;
      
      public var spcId:int = 0;
      
      public var captainLimit:int = 0;
      
      public function SeasonPassRewardUnclaimedInfoCommand(param1:int = 0, param2:String = "", param3:String = "", param4:Boolean = false, param5:int = 0, param6:Number = 0, param7:int = 0)
      {
         super();
         this.marketingImage = param3;
         this.captainWarning = param4;
         this.captainOwned = param6;
         this.seasonName = param2;
         this.captainPending = param5;
         this.spcId = param1;
         this.captainLimit = param7;
      }
      
      public static function get instance() : SeasonPassRewardUnclaimedInfoCommand
      {
         return _instance || (_instance = new SeasonPassRewardUnclaimedInfoCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return 9608;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 25;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.marketingImage = param1.readUTF();
         this.captainWarning = param1.readBoolean();
         this.captainOwned = param1.readDouble();
         this.seasonName = param1.readUTF();
         this.captainPending = param1.readInt();
         this.captainPending = this.captainPending << 10 | this.captainPending >>> 22;
         this.spcId = param1.readInt();
         this.spcId = this.spcId >>> 16 | this.spcId << 16;
         this.captainLimit = param1.readInt();
         this.captainLimit = this.captainLimit >>> 11 | this.captainLimit << 21;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(9608);
         param1.writeUTF(this.marketingImage);
         param1.writeBoolean(this.captainWarning);
         param1.writeDouble(this.captainOwned);
         param1.writeUTF(this.seasonName);
         param1.writeInt(this.captainPending >>> 10 | this.captainPending << 22);
         param1.writeInt(this.spcId << 16 | this.spcId >>> 16);
         param1.writeInt(this.captainLimit << 11 | this.captainLimit >>> 21);
      }
   }
}

