import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:t9n_manager_flutter_client/domains/tenant/apis/api_tenant.dart';
import 'package:t9n_manager_flutter_client/shared/alert.dart';
import 'package:t9n_manager_flutter_client/shared/app_state_notifier.dart';
import '../../../generated/l10n.dart';
import '../../../shared/app_settings.dart';
import '../../../shared/models/api_exception.dart';
import '../models/tenant.dart';
import '../widgets/tenant_card.dart';

class TenantScreen extends StatefulWidget {
  const TenantScreen({Key? key}) : super(key: key);

  @override
  State<TenantScreen> createState() => _TenantScreenState();
}

class _TenantScreenState extends State<TenantScreen> {
  @override
  Widget build(BuildContext context) {
    AppSettings appSettings = context.watch<AppSettings>();
    AppState appState = context.watch<AppState>();
    var jwt = appState.jwt;
    return FutureBuilder<List<Tenant>>(
        future: getAllTenants(appSettings, jwt, context),
        builder: (context, AsyncSnapshot<List<Tenant>> snapshot) {
          if (snapshot.connectionState == ConnectionState.waiting) {
            return const CircularProgressIndicator();
          } else if (snapshot.hasData) {
            return Scaffold(
              appBar: AppBar(
                title: Text(S.of(context).title),
              ),
              body: getTenantCards(snapshot.requireData),
            );
          } else if (snapshot.hasError) {
            ApiException e = snapshot.error as ApiException;
            return alert(S.of(context).api_error_title, e.t9nMessage, S.of(context).common_button_ok, context);
          } else {
            return const Text("");
          }
        });
  }

  Widget getTenantCards(List<Tenant> tenants) {
    try {
      List<Widget> listTenantCards = <Widget>[];
      for (var counter = 0; counter < tenants.length; counter++) {
        listTenantCards.add(TenantCard(tenants[counter], false));
      }
      listTenantCards.add(TenantCard(Tenant("_", S.of(context).tenant_add_button), true));
      return Row(
        children: listTenantCards,
      );
    } catch (e) {
      rethrow;
    }
  }
}
