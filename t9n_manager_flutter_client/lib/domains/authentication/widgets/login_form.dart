import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:t9n_manager_flutter_client/domains/authentication/apis/api_authentication.dart';
import 'package:t9n_manager_flutter_client/domains/authentication/models/user_login_model.dart';
import 'package:t9n_manager_flutter_client/shared/app_settings.dart';

import '../../../generated/l10n.dart';
import '../../../shared/app_state_notifier.dart';
import '../../../shared/widgets/screen_title.dart';
import '../../../shared/widgets/spacer.dart';

class LoginForm extends StatefulWidget {
  const LoginForm({Key? key}) : super(key: key);

  @override
  _LoginFormState createState() => _LoginFormState();
}

class _LoginFormState extends State<LoginForm> {
  final _formKey = GlobalKey<FormState>();
  TextEditingController txtLogin = TextEditingController();
  TextEditingController txtPassword = TextEditingController();

  bool _showPassword = false;
  @override
  Widget build(BuildContext context) {
    AppSettings appSettings = context.watch<AppSettings>();
    double width = MediaQuery.of(context).size.width > context.watch<AppState>().screenMaxSize ? context.watch<AppState>().screenMaxSize : MediaQuery.of(context).size.width;
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
                ScreenTitle(S.of(context).login_form_title),
                Text(
                  S.of(context).login_form_subtitle,
                  style: const TextStyle(fontSize: 12, fontWeight: FontWeight.normal),
                ),
                verticalSpaceTiny,
                TextFormField(
                  validator: (String? value) {
                    if (value!.isEmpty) {
                      return S.of(context).login_form_validator_login;
                    }
                    return null;
                  },
                  controller: txtLogin,
                  decoration: InputDecoration(
                    label: Text(S.of(context).login_form_textfieldlabel_login),
                  ),
                ),
                TextFormField(
                  validator: (String? value) {
                    if (value == "") {
                      return S.of(context).login_form_validator_password;
                    }
                    return null;
                  },
                  controller: txtPassword,
                  decoration: InputDecoration(
                      label: Text(S.of(context).login_form_textfieldlabel_password),
                      suffixIcon: IconButton(
                        onPressed: () {
                          setState(() {
                            _showPassword = !_showPassword;
                          });
                        },
                        icon: Icon(_showPassword ? Icons.visibility_off : Icons.visibility),
                      )),
                  obscureText: !_showPassword,
                ),
                verticalSpaceSmall,
                Align(
                  alignment: Alignment.centerRight,
                  child: InkWell(
                    child: Text(
                      S.of(context).login_form_label_forgotpassword,
                      style: const TextStyle(decoration: TextDecoration.underline),
                    ),
                    onTap: () {
                      Navigator.popAndPushNamed(context, '/forgotpassword');
                    },
                  ),
                ),
                verticalSpaceTiny,
                ElevatedButton(
                    onPressed: () {
                      if (_formKey.currentState!.validate()) {
                        UserLoginModel userLoginModel = UserLoginModel(txtLogin.text, txtPassword.text);
                        postLogin(userLoginModel, context).then((value) {
                          if (value.httpStatus == 200) {
                            context.read<AppState>().setToken(value.message);
                            context.read<AppState>().logIn();
                            Navigator.popAndPushNamed(context, "/");
                          } else {
                            alert(S.of(context).login_form_msg_login_failed_title, value.message + "-" + value.moreInfo, S.of(context).common_button_ok);
                          }
                        });
                      }
                    },
                    child: Text(S.of(context).login_form_button_label_login)),
                verticalSpaceSmall,
                Text(S.of(context).login_form_label_noaccount),
                InkWell(
                  child: Text(S.of(context).login_form_label_signup, style: const TextStyle(decoration: TextDecoration.underline)),
                  onTap: () {
                    Navigator.popAndPushNamed(context, "/signup");
                  },
                ),
              ],
            ),
          ),
        ),
      ),
    );
  }

  alert(String title, String message, String buttonLabel) {
    showDialog<String>(
        context: context,
        builder: (BuildContext context) => AlertDialog(
            shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(15)),
            title: Text(title),
            content: Text(message),
            actions: <Widget>[TextButton(onPressed: () => Navigator.pop(context, buttonLabel), child: Text(buttonLabel))]));
  }
}
