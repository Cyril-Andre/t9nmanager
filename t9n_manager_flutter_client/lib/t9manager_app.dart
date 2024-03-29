import 'package:flutter/material.dart';
import 'package:flutter_localizations/flutter_localizations.dart';
import 'package:provider/provider.dart';
import 'package:t9n_manager_flutter_client/domains/project/screens/project_screen.dart';

import 'package:t9n_manager_flutter_client/domains/arb_management/screens/arb_management_screen.dart';
import 'package:t9n_manager_flutter_client/domains/authentication/screens/forgot_password_screen.dart';
import 'package:t9n_manager_flutter_client/domains/arb_management/screens/home_screen.dart';
import 'package:t9n_manager_flutter_client/domains/authentication/screens/login_screen.dart';
import 'package:t9n_manager_flutter_client/domains/authentication/screens/signup_screen.dart';
import 'package:t9n_manager_flutter_client/domains/tenant/screens/tenant_screen.dart';
import 'package:t9n_manager_flutter_client/generated/l10n.dart';
import 'package:t9n_manager_flutter_client/shared/app_state_notifier.dart';

// ignore: camel_case_types
class t9nManagerApp extends StatelessWidget {
  const t9nManagerApp({Key? key}) : super(key: key);

  // This widget is the root of your application.
  @override
  Widget build(BuildContext context) {
//    var tenantPublic = context.watch<AppSettings>().publicTenant;
//    context.read<AppState>().setTenant(tenantPublic);
    return MaterialApp(
      debugShowCheckedModeBanner: false,
      localizationsDelegates: const [S.delegate, GlobalMaterialLocalizations.delegate, GlobalCupertinoLocalizations.delegate, GlobalWidgetsLocalizations.delegate],
      supportedLocales: S.delegate.supportedLocales,
      title: 't9n Manager',
      theme: ThemeData(
        // This is the theme of your application.
        //
        // Try running your application with "flutter run". You'll see the
        // application has a blue toolbar. Then, without quitting the app, try
        // changing the primarySwatch below to Colors.green and then invoke
        // "hot reload" (press "r" in the console where you ran "flutter run",
        // or simply save your changes to "hot reload" in a Flutter IDE).
        // Notice that the counter didn't reset back to zero; the application
        // is not restarted.
        primarySwatch: Colors.blue,
      ),
      routes: {
        '/': (context) => context.watch<AppState>().isLoggedIn ? const HomeScreen() : const LoginScreen(),
        '/relog': (context) => const LoginScreen(),
        '/signup': (context) => const SignUpScreen(),
        '/forgotpassword': (context) => const ForgotPasswordScreen(),
        '/tenant': (context) => const TenantScreen(),
        '/project': (context) => const ProjectScreen(),
        '/arb_management':(context)=> const ArbManagementScreen(),
      },
      initialRoute: "/",
    );
  }
}
