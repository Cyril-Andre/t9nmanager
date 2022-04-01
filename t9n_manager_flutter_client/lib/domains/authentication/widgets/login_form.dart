import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

import '../../../generated/l10n.dart';
import '../../../main.dart';
import '../../../shared/widgets/screen_title.dart';
import '../../../shared/widgets/spacer.dart';


class LoginForm extends StatefulWidget {
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
    double width = MediaQuery.of(context).size.width>context.watch<AppState>().ScreenMaxSize?context.watch<AppState>().ScreenMaxSize:MediaQuery.of(context).size.width;
    double height = MediaQuery.of(context).size.height;
    return 
       Center(
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
                  ScreenTitle(S.of(context).login_screen_title),
                  Text(S.of(context).login_screen_subtitle, style: const TextStyle(fontSize: 12, fontWeight: FontWeight.normal),
                  ),
                  verticalSpaceTiny,
                  TextFormField(
                    validator: (String? value) {
                      if (value!.isEmpty) return S.of(context).login_screen_validator_login;
                      return null;
                    },
                    controller: txtLogin,
                    decoration: InputDecoration(
                      label: Text(S.of(context).login_screen_textfieldlabel_login),
                    ),
                  ),
                  TextFormField(
                    validator: (String? value) {
                      if (value=="") return S.of(context).login_screen_validator_password;
                      return null;
                    },
                    controller: txtPassword,
                    decoration: InputDecoration(
                        label:  Text(S.of(context).login_screen_textfieldlabel_password),
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
                  Align(
                    alignment: Alignment.centerRight,
                    child: InkWell(
                      child: Text(
                        S.of(context).login_screen_label_forgotpassword,
                        style:const TextStyle(decoration: TextDecoration.underline),
                      ),
                      onTap: () {
                        Navigator.popAndPushNamed(context, '/forgotpassword');
                      },
                    ),
                  ),
                  verticalSpaceTiny,
                  ElevatedButton(
                      onPressed: () {
                        if(_formKey.currentState!.validate()){
                          context.read<AppState>().toggleLogedIn();
                        }
                      },
                      child: Text(S.of(context).login_screen_button_label_login)),
                  verticalSpaceSmall,
                    Text(S.of(context).login_screen_label_noaccount),
                    InkWell(
                      child: Text(S.of(context).login_screen_label_signup,style: const TextStyle(decoration: TextDecoration.underline)),
                      onTap: () {
                        Navigator.popAndPushNamed(context, "/signup");
                      },
                    ),
                ],
              ),
            ),
          ),
        ),
      )
    ;
  }
}
