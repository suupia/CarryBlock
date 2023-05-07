using Fusion;

public class Ball : NetworkBehaviour
{
    [Networked] TickTimer life { get; set; }

    public void Init()
    {
        //�T�b���TickTimer���擾�ł���
        life = TickTimer.CreateFromSeconds(Runner, 5.0f);
    }

    public override void FixedUpdateNetwork()
    {
        if (life.Expired(Runner))
            Runner.Despawn(Object);
        else
            //Interpolation Data Source��Predicted�ɂ���ƁA���[�J���ŗ\�����Ă����
            //�\���̓T�[�o�[�̃X�i�b�v�V���b�g���琔�񕪒��x�ŁA�v���C���[�̓��͂��Ȃ��ꍇ�͂�����x���m
            //����ɂ���āA��x���𑕂��Ă���
            transform.position += 5 * Runner.DeltaTime * transform.forward;
    }
}