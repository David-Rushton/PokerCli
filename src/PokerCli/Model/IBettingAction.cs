namespace PokerCli.Model
{
    public enum bettingAction
    {
        Check,
        Call,
        AllIn,
        Raise,
        Fold
    }


    public interface IBettingAction
    {
        public (bettingAction BettingAction, decimal Stake) GetBet(Player player, decimal highestRaise);
    }
}
