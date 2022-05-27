import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:t9n_manager_flutter_client/domains/authentication/apis/api_authentication.dart';
import 'package:t9n_manager_flutter_client/shared/app_settings.dart';

import '../../../generated/l10n.dart';
import '../../../shared/app_state_notifier.dart';
import '../../../shared/widgets/screen_title.dart';
import '../../../shared/widgets/spacer.dart';
import '../models/user_reinit_password_model.dart';

class ForgotPasswordForm extends StatefulWidget {
  const ForgotPasswordForm({Key? key}) : super(key: key);
  @override
  _ForgotPasswordFormState createState() => _ForgotPasswordFormState();
}

class _ForgotPasswordFormState extends State<ForgotPasswordForm> {
  final _formKey = GlobalKey<FormState>();
  TextEditingController txtEmail = TextEditingController();
  TextEditingController txtPassword = TextEditingController();
  TextEditingController txtPasswordConfirm = TextEditingController();
  TextEditingController txtOtp = TextEditingController();
  bool _showPassword = false;
  bool _showPasswordConfirm = false;
  bool _otpPending = true;

  @override
  Widget build(BuildContext context) {
    AppSettings appSettings = context.watch<AppSettings>();
    double width = MediaQuery.of(context).size.width >context.watch<AppState>().screenMaxSize ? context.watch<AppState>().screenMaxSize
                                                                                              : MediaQuery.of(context).size.width;
    double height = MediaQuery.of(context).size.height;
    return Center(
      child: SizedBox(
        width: .8 * width,
        height: height,
        child: Form(
          key: _formKey,
          autovalidateMode: AutovalidateMode.onUserInteraction,
          child: SingleChildScrollView(
            child: Column(
              mainAxisAlignment: MainAxisAlignment.start,
              crossAxisAlignment: CrossAxisAlignment.center,
              children: [
                verticalSpaceMedium,
                ScreenTitle(S.of(context).forgot_password_form_title),
                Text( //Subtitle
                  S.of(context).forgot_password_form_subtitle,
                  style: const TextStyle(
                      fontSize: 12, fontWeight: FontWeight.normal),
                ),
                verticalSpaceTiny,
                TextFormField( // Email
                  keyboardType: TextInputType.emailAddress,
                  validator: (String? value) {
                    if (value!.isEmpty) {
                      return S.of(context).forgot_password_form_validator_email;
                    }
                    return null;
                  },
                  controller: txtEmail,
                  readOnly: !_otpPending,
                  decoration: InputDecoration(
                    label: Text(S
                        .of(context)
                        .forgot_password_form_textfieldlabel_email),
                  ),
                ),
                verticalSpaceTiny,
                ElevatedButton( // Ask OTP button
                    onPressed: !_otpPending
                        ? null
                        : () {
                            if (_formKey.currentState!.validate()) {
                              UserResetPasswordModel userReinitPasswordModel =
                                  UserResetPasswordModel(
                                      txtEmail.text, null, null);
                              postResetPassword1(
                                      userReinitPasswordModel, appSettings)
                                  .then((value) {
                                if (value.httpStatus == 200) {
                                  alert(
                                      S
                                          .of(context)
                                          .forgot_password_form_msg_check_email_title,
                                      S
                                          .of(context)
                                          .forgot_password_form_msg_check_email_message,
                                      S.of(context).common_button_ok);
                                  setState(() {
                                    _otpPending = false;
                                  });
                                } else {
                                  alert(
                                      S
                                          .of(context)
                                          .forgot_password_form_msg_fail_title,
                                      value.message + "-" + value.moreInfo,
                                      S.of(context).common_button_ok);
                                }
                              });
                            }
                          },
                    child: Text(
                        S.of(context).forgot_password_form_button_label_otp)),
                verticalSpaceSmall,
                Container( // Password and OTP confirmation subform
                    child: _otpPending
                        ? null
                        : Column(
                            children: [
                              //Password
                              TextFormField(
                                validator: (String? value) {
                                  if (value == "") {
                                    return S
                                        .of(context)
                                        .signup_form_validator_password;
                                  }
                                  return null;
                                },
                                controller: txtPassword,
                                decoration: InputDecoration(
                                    label: Text(S
                                        .of(context)
                                        .signup_form_textfieldlabel_password),
                                    suffixIcon: IconButton(
                                      onPressed: () {
                                        setState(() {
                                          _showPassword = !_showPassword;
                                        });
                                      },
                                      icon: Icon(_showPassword
                                          ? Icons.visibility_off
                                          : Icons.visibility),
                                    )),
                                obscureText: !_showPassword,
                              ),
                              verticalSpaceSmall,
                              //Password confimation
                              TextFormField(
                                validator: (String? value) {
                                  if (value != txtPassword.text) {
                                    return S
                                        .of(context)
                                        .signup_form_validator_password_confirm;
                                  }
                                  return null;
                                },
                                controller: txtPasswordConfirm,
                                decoration: InputDecoration(
                                    label: Text(S
                                        .of(context)
                                        .signup_form_textfieldlabel_password_confirm),
                                    suffixIcon: IconButton(
                                      onPressed: () {
                                        setState(() {
                                          _showPasswordConfirm =
                                              !_showPasswordConfirm;
                                        });
                                      },
                                      icon: Icon(_showPasswordConfirm
                                          ? Icons.visibility_off
                                          : Icons.visibility),
                                    )),
                                obscureText: !_showPasswordConfirm,
                              ),
                              verticalSpaceSmall,
                              //OTP
                              TextFormField(
                                validator: (String? value) {
                                  if (value == "") {
                                    return S
                                        .of(context)
                                        .signup_form_validator_otp;
                                  }
                                  return null;
                                },
                                controller: txtOtp,
                                decoration: InputDecoration(
                                  label: Text(S
                                      .of(context)
                                      .signup_form_textfieldlabel_otp),
                                ),
                                obscureText: !_showPassword,
                              ),
                              verticalSpaceSmall,
                              // Reset password button
                              ElevatedButton(
                                  onPressed: () {
                                    if (_formKey.currentState!.validate()) {
                                      UserResetPasswordModel
                                          userReinitPasswordModel =
                                          UserResetPasswordModel(txtEmail.text,
                                              txtOtp.text, txtPassword.text);
                                      postResetPassword2(
                                              userReinitPasswordModel,
                                              appSettings)
                                          .then((value) async {
                                        if (value.httpStatus == 200) {
                                          await alert(
                                                  S
                                                      .of(context)
                                                      .forgot_password_form_msg_password_reset_success_title,
                                                  S
                                                      .of(context)
                                                      .forgot_password_form_msg_password_reset_success_message,
                                                  S
                                                      .of(context)
                                                      .common_button_ok);
                                             
                                            Navigator.popAndPushNamed(context, '/');                                          
                                        } else {
                                          alert(
                                              S
                                                  .of(context)
                                                  .forgot_password_form_msg_password_reset_fail_title,
                                              value.message +
                                                  "-" +
                                                  value.moreInfo,
                                              S.of(context).common_button_ok);
                                        }
                                      });
                                    }
                                  },
                                  child: Text(S
                                      .of(context)
                                      .forgot_password_form_button_label_reset)),
                              verticalSpaceSmall,
                            ],
                          )),
                Text(S.of(context).forgot_password_form_label_remember),
                // Switch to login screen
                InkWell(
                  child: Text(S.of(context).forgot_password_form_label_login,
                      style: const TextStyle(
                          decoration: TextDecoration.underline)),
                  onTap: () {
                    Navigator.popAndPushNamed(context, "/");
                  },
                ),
              ],
            ),
          ),
        ),
      ),
    );
  }

  Future<String?> alert(String title, String message, String buttonLabel) async {
    return (showDialog<String>(
        context: context,
        builder: (BuildContext context) => AlertDialog(
                shape: RoundedRectangleBorder(
                    borderRadius: BorderRadius.circular(15)),
                title: Text(title),
                content: Text(message),
                actions: <Widget>[
                  TextButton(
                      onPressed: () => Navigator.pop(context, buttonLabel),
                      child: Text(buttonLabel))
                ])));
  }
}
