using System;
using System.Windows.Forms;
using ZDC2911Demo.IConvert;
using ZDC2911Demo.SysEnum;
using Riss.Devices;

namespace ZDC2911Demo.UI {
    public partial class EnrollDetailForm : Form {
        private User user;

        public EnrollDetailForm(User user) {
            InitializeComponent();
            this.user = user;
        }

        private void SetPrivilege() {
            switch (user.Privilege) {
                case (int)UserPrivilege.ROLE_GENERAL_USER:
                    cbo_Role.SelectedIndex = 0;
                    break;

                case (int)UserPrivilege.ROLE_SUPER_USER:
                    cbo_Role.SelectedIndex = 1;
                    break;

                case (int)UserPrivilege.ROLE_ENROLL_USER:
                    cbo_Role.SelectedIndex = 2;
                    break;

                case (int)UserPrivilege.ROLE_VIEW_USER:
                    cbo_Role.SelectedIndex = 3;
                    break;

                case (int)UserPrivilege.ROLE_CUSTOMER:
                    cbo_Role.SelectedIndex = 4;
                    break;
            }
        }

        private void EnrollDetailForm_Load(object sender, EventArgs e) {
            txt_UserId.Text = user.DIN.ToString();
            Enroll enroll = user.Enrolls[0];
            SetPrivilege();

            for (int i = 0; i < Zd2911Utils.MaxFingerprintCount; i++) {
                if (0 != Zd2911Utils.BitCheck((int)enroll.EnrollType, i)) {
                    clb_Fp.SetItemChecked(i, true);
                }
            }

            if (0 != Zd2911Utils.BitCheck((int)enroll.EnrollType, 10)) {
                clb_Fp.SetItemChecked(10, true);
            }

            if (0 != Zd2911Utils.BitCheck((int)enroll.EnrollType, 11)) {
                clb_Fp.SetItemChecked(11, true);
            }
        }
    }
}
